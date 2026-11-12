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
