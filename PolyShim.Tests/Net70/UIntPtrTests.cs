using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class UIntPtrTests
{
#if !NETFRAMEWORK
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        UIntPtr.TryParse("42".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(new UIntPtr(42));

        UIntPtr.TryParse("-1".AsSpan(), null, out _).Should().BeFalse();
    }
#endif
}
