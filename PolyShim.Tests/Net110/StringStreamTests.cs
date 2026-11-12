using System;
using System.IO;
using System.Linq;
using System.Text;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class StringStreamTests
{
    [Fact]
    public void Read_Test()
    {
        // Arrange
        const string text = "Hello, wörld! \U0001F600";
        using var stream = new StringStream(text, Encoding.UTF8);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        // Act
        var result = reader.ReadToEnd();

        // Assert
        result.Should().Be(text);
    }

    [Fact]
    public void Read_Empty_Test()
    {
        // Arrange
        const string text = "";
        using var stream = new StringStream(text, Encoding.UTF8);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        // Act
        var result = reader.ReadToEnd();

        // Assert
        result.Should().Be(text);
    }

    [Fact]
    public void Read_Large_Test()
    {
        // Arrange
        // Long enough to span multiple internal chunks, with multi-byte characters
        // (including surrogate pairs) sitting on and around the chunk boundaries
        var text = string.Concat(Enumerable.Repeat("x\U0001F600ö", 5000)) + "\uD83D";
        using var stream = new StringStream(text, Encoding.UTF8);
        using var destination = new MemoryStream();

        // Act
        stream.CopyTo(destination);

        // Assert
        destination.ToArray().Should().Equal(Encoding.UTF8.GetBytes(text));
    }

    [Fact]
    public void Read_InvalidInput_Test()
    {
        // Arrange
        // The encoding error must surface before the stream is fully read. Exactly when it
        // surfaces is implementation-defined (the native implementation encodes in chunks,
        // so it may throw before the valid prefix is fully consumed), so only the bytes
        // successfully read before the error are verified.
        var text = new string('a', 1023) + "\uD800";
        using var stream = new StringStream(text, new UTF8Encoding(false, true));
        var buffer = new byte[1];
        var bytesRead = 0;

        // Act
        var act = () =>
        {
            while (stream.Read(buffer, 0, 1) == 1)
            {
                buffer[0].Should().Be((byte)'a');
                bytesRead++;
            }
        };

        // Assert
        act.Should().Throw<EncoderFallbackException>();
        bytesRead.Should().BeLessThanOrEqualTo(1023);
    }

    [Fact]
    public void Dispose_Test()
    {
        // Arrange
        var stream = new StringStream("Hello, world!", Encoding.UTF8);

        // Act
        stream.Dispose();

        // Assert
        stream.CanRead.Should().BeFalse();

        var act = () => stream.Read(new byte[16], 0, 16);
        act.Should().Throw<ObjectDisposedException>();
    }
}
