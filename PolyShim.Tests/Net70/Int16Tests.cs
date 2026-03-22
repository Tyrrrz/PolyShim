using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class Int16Tests
{
#if FEATURE_MEMORY
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        short.TryParse("1000".AsSpan(), null, out var result).Should().BeTrue();
        result.Should().Be(1000);

        short.TryParse("99999".AsSpan(), null, out _).Should().BeFalse();
    }
#endif
}
