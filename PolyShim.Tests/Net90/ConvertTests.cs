using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net90;

public class ConvertTests
{
    [Fact]
    public void ToHexStringLower_Test()
    {
        // Act & assert
        Convert.ToHexStringLower("John"u8.ToArray()).Should().Be("4a6f686e");
        Convert.ToHexStringLower(Array.Empty<byte>()).Should().Be("");
        Convert.ToHexStringLower("John"u8.ToArray(), 1, 2).Should().Be("6f68");
    }
}
