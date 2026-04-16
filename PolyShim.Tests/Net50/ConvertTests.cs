using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class ConvertTests
{
    [Fact]
    public void FromHexString_String_Test()
    {
        // Act & assert
        Convert.FromHexString("4A6F686E").Should().Equal("John"u8.ToArray());
        Convert.FromHexString("4a6f686e").Should().Equal("John"u8.ToArray());
        Convert.FromHexString("4A6f686E").Should().Equal("John"u8.ToArray());
        Convert.FromHexString("").Should().BeEmpty();
        Assert.Throws<FormatException>(() => Convert.FromHexString("4A6F686")); // Odd length
        Assert.Throws<FormatException>(() => Convert.FromHexString("4A6F68GH")); // Invalid character
    }

#if !NETFRAMEWORK
    [Fact]
    public void FromHexString_Span_Test()
    {
        // Act & assert
        Convert.FromHexString("4A6F686E".AsSpan()).Should().Equal("John"u8.ToArray());
        Convert.FromHexString("4a6f686e".AsSpan()).Should().Equal("John"u8.ToArray());
        Convert.FromHexString("".AsSpan()).Should().BeEmpty();
    }
#endif

    [Fact]
    public void ToHexString_Array_Test()
    {
        // Act & assert
        Convert.ToHexString("John"u8.ToArray()).Should().Be("4A6F686E");
        Convert.ToHexString([]).Should().Be("");
        Convert.ToHexString("John"u8.ToArray(), 1, 2).Should().Be("6F68");
    }

    [Fact]
    public void ToHexString_Span_Test()
    {
        // Act & assert
        Convert.ToHexString("John"u8).Should().Be("4A6F686E");
        Convert.ToHexString(ReadOnlySpan<byte>.Empty).Should().Be("");
    }
}
