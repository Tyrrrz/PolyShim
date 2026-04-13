using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class GuidTests
{
    private static readonly Guid SampleGuid = new Guid("d3b16c2f-3e7d-4e7f-a012-3456789abcde");

    [Fact]
    public void TryParse_ValidString_ReturnsTrue()
    {
        // Act & assert
        Guid.TryParse("d3b16c2f-3e7d-4e7f-a012-3456789abcde", out var result).Should().BeTrue();
        result.Should().Be(SampleGuid);
    }

    [Fact]
    public void TryParse_InvalidString_ReturnsFalse()
    {
        // Act & assert
        Guid.TryParse("not-a-guid", out _).Should().BeFalse();
    }

    [Fact]
    public void TryParse_NullInput_ReturnsFalse()
    {
        // Act & assert
        Guid.TryParse((string?)null, out _).Should().BeFalse();
    }

    [Fact]
    public void TryParseExact_FormatN_ReturnsTrue()
    {
        // Act & assert
        Guid.TryParseExact("d3b16c2f3e7d4e7fa0123456789abcde", "N", out var result)
            .Should()
            .BeTrue();
        result.Should().Be(SampleGuid);
    }

    [Fact]
    public void TryParseExact_FormatD_ReturnsTrue()
    {
        // Act & assert
        Guid.TryParseExact("d3b16c2f-3e7d-4e7f-a012-3456789abcde", "D", out var result)
            .Should()
            .BeTrue();
        result.Should().Be(SampleGuid);
    }

    [Fact]
    public void TryParseExact_FormatB_ReturnsTrue()
    {
        // Act & assert
        Guid.TryParseExact("{d3b16c2f-3e7d-4e7f-a012-3456789abcde}", "B", out var result)
            .Should()
            .BeTrue();
        result.Should().Be(SampleGuid);
    }

    [Fact]
    public void TryParseExact_FormatP_ReturnsTrue()
    {
        // Act & assert
        Guid.TryParseExact("(d3b16c2f-3e7d-4e7f-a012-3456789abcde)", "P", out var result)
            .Should()
            .BeTrue();
        result.Should().Be(SampleGuid);
    }

    [Fact]
    public void TryParseExact_FormatX_ReturnsTrue()
    {
        // Act & assert
        Guid.TryParseExact(
                "{0xd3b16c2f,0x3e7d,0x4e7f,{0xa0,0x12,0x34,0x56,0x78,0x9a,0xbc,0xde}}",
                "X",
                out var result
            )
            .Should()
            .BeTrue();
        result.Should().Be(SampleGuid);
    }

    [Fact]
    public void TryParseExact_WrongFormat_ReturnsFalse()
    {
        // Act & assert
        Guid.TryParseExact("d3b16c2f-3e7d-4e7f-a012-3456789abcde", "N", out _).Should().BeFalse();
    }

    [Fact]
    public void TryParseExact_InvalidFormat_ReturnsFalse()
    {
        // Act & assert
        Guid.TryParseExact("d3b16c2f-3e7d-4e7f-a012-3456789abcde", "Z", out _).Should().BeFalse();
    }

    [Fact]
    public void TryParseExact_NullInput_ReturnsFalse()
    {
        // Act & assert
        Guid.TryParseExact(null, "D", out _).Should().BeFalse();
    }

    [Fact]
    public void TryParseExact_NullFormat_ReturnsFalse()
    {
        // Act & assert
        Guid.TryParseExact("d3b16c2f-3e7d-4e7f-a012-3456789abcde", null, out _).Should().BeFalse();
    }
}
