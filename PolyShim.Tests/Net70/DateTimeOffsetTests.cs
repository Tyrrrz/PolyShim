using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class DateTimeOffsetTests
{
#if FEATURE_MEMORY
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        DateTimeOffset
            .TryParse(
                "2024-01-15T12:00:00+00:00".AsSpan(),
                CultureInfo.InvariantCulture,
                out var result
            )
            .Should()
            .BeTrue();
        result.Year.Should().Be(2024);
        result.Month.Should().Be(1);
        result.Day.Should().Be(15);

        DateTimeOffset.TryParse("not a date".AsSpan(), null, out _).Should().BeFalse();
    }
#endif
}
