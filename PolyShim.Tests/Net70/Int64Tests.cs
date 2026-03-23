using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class Int64Tests
{
#if !NETFRAMEWORK
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        long.TryParse("9876543210".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(9876543210L);

        long.TryParse("abc".AsSpan(), null, out _).Should().BeFalse();
    }
#endif
}
