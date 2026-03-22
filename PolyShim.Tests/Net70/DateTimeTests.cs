using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class DateTimeTests
{
    [Fact]
    public void TryParse_Span_Test()
    {
        // Act & assert
        DateTime
            .TryParse("2024-01-15".AsSpan(), CultureInfo.InvariantCulture, out var result)
            .Should()
            .BeTrue();
        result.Year.Should().Be(2024);
        result.Month.Should().Be(1);
        result.Day.Should().Be(15);

        DateTime.TryParse("not a date".AsSpan(), null, out _).Should().BeFalse();
    }
}
