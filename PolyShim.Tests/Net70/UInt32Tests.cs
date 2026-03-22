using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class UInt32Tests
{
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        uint.TryParse("4000000000".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(4000000000U);
    }
}
