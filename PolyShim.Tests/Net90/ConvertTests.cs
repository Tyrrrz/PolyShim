using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net90;

public class ConvertTests
{
    [Fact]
    public void ToHexStringLower_Array_Test()
    {
        // Act & assert
        Convert.ToHexStringLower("John"u8.ToArray()).Should().Be("4a6f686e");
        Convert.ToHexStringLower([]).Should().Be("");
        Convert.ToHexStringLower("John"u8.ToArray(), 1, 2).Should().Be("6f68");
    }

    [Fact]
    public void ToHexStringLower_Span_Test()
    {
        // Act & assert
        Convert.ToHexStringLower("John"u8).Should().Be("4a6f686e");
        Convert.ToHexStringLower(ReadOnlySpan<byte>.Empty).Should().Be("");
    }
}
