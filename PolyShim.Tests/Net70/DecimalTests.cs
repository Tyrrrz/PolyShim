using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class DecimalTests
{
#if FEATURE_MEMORY
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        decimal.TryParse("123.45".AsSpan(), CultureInfo.InvariantCulture, out var result)
            .Should()
            .BeTrue();
        result.Should().Be(123.45m);

        decimal.TryParse("abc".AsSpan(), null, out _).Should().BeFalse();
    }
#endif
}
