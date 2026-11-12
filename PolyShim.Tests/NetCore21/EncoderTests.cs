using System;
using System.Text;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class EncoderTests
{
    [Fact]
    public void Convert_Test()
    {
        // Arrange
        const string text = "Hello, wörld! \U0001F600";
        var encoder = Encoding.UTF8.GetEncoder();
        var buffer = new byte[Encoding.UTF8.GetMaxByteCount(text.Length)];

        // Act
        encoder.Convert(
            text.AsSpan(),
            buffer.AsSpan(),
            true,
            out var charsUsed,
            out var bytesUsed,
            out var completed
        );

        // Assert
        charsUsed.Should().Be(text.Length);
        completed.Should().BeTrue();
        buffer.AsSpan(0, bytesUsed).ToArray().Should().Equal(Encoding.UTF8.GetBytes(text));
    }

    [Fact]
    public void Convert_Partial_Test()
    {
        // Arrange
        const string text = "abc";
        var encoder = Encoding.UTF8.GetEncoder();
        var buffer = new byte[2];

        // Act
        encoder.Convert(
            text.AsSpan(),
            buffer.AsSpan(),
            true,
            out var charsUsed,
            out var bytesUsed,
            out var completed
        );

        // Assert
        charsUsed.Should().Be(2);
        bytesUsed.Should().Be(2);
        completed.Should().BeFalse();
        buffer.Should().Equal("ab"u8.ToArray());
    }
}
