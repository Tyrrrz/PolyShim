using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class UInt64Tests
{
#if !NETFRAMEWORK
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        ulong.TryParse("18000000000000000000".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(18000000000000000000UL);

        ulong.TryParse("-1".AsSpan(), null, out _).Should().BeFalse();
    }
#endif
}
