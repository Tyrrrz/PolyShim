using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class EnumTests
{
    [Fact]
    public void IsDefined_Test()
    {
        // Act & assert
        Enum.IsDefined((DayOfWeek)0).Should().BeTrue();
        Enum.IsDefined((DayOfWeek)3).Should().BeTrue();
        Enum.IsDefined((DayOfWeek)7).Should().BeFalse();
    }

    [Fact]
    public void GetNames_Test()
    {
        // Act & assert
        Enum.GetNames<DayOfWeek>()
            .Should()
            .Equal("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");
    }

    [Fact]
    public void GetValues_Test()
    {
        // Act & assert
        Enum.GetValues<DayOfWeek>()
            .Should()
            .Equal(
                DayOfWeek.Sunday,
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday
            );
    }
}
