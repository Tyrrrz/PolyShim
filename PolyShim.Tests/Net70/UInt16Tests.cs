using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class UInt16Tests
{
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        ushort.TryParse("1000".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(1000);

        ushort.TryParse("-1".AsSpan(), null, out _).Should().BeFalse();
    }
}
