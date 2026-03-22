using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class Int32Tests
{
#if FEATURE_MEMORY
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        int.TryParse("12345".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(12345);

        int.TryParse("abc".AsSpan(), null, out _).Should().BeFalse();
    }
#endif
}
