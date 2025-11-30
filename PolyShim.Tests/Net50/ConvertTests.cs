using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class ConvertTests
{
    [Fact]
    public void FromHexString_Test()
    {
        // Act & assert
        Convert.FromHexString("4A6F686E").Should().Equal("John"u8.ToArray());
        Convert.FromHexString("").Should().BeEmpty();
        Assert.Throws<FormatException>(() => Convert.FromHexString("4A6F686")); // Odd length
        Assert.Throws<FormatException>(() => Convert.FromHexString("4A6F68GH")); // Invalid character
    }

    [Fact]
    public void ToHexString_Test()
    {
        // Act & assert
        Convert.ToHexString("John"u8.ToArray()).Should().Be("4A6F686E");
        Convert.ToHexString(Array.Empty<byte>()).Should().Be("");
        Convert.ToHexString("John"u8.ToArray(), 1, 2).Should().Be("6F68");
    }
}
