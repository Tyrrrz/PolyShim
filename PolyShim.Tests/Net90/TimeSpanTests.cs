using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net90;

public class TimeSpanTests
{
    [Fact]
    public void FromMilliseconds_TwoArgs_Test()
    {
        // Act & assert
        TimeSpan.FromMilliseconds(1234L, 567L).Should().Be(TimeSpan.FromTicks(12_345_670));
        TimeSpan.FromMilliseconds(0L, 0L).Should().Be(TimeSpan.Zero);
        TimeSpan.FromMilliseconds(-1000L, -500L).Should().Be(TimeSpan.FromTicks(-10_005_000));
    }

    [Fact]
    public void FromSeconds_Test()
    {
        // Act & assert
        TimeSpan.FromSeconds(1L, 500L, 0L).Should().Be(TimeSpan.FromMilliseconds(1500));
        TimeSpan.FromSeconds(1L, 500L, 250L).Should().Be(TimeSpan.FromTicks(15_002_500));
    }

    [Fact]
    public void FromMinutes_Test()
    {
        // Act & assert
        TimeSpan.FromMinutes(1L, 30L, 0L, 0L).Should().Be(TimeSpan.FromSeconds(90));
        TimeSpan.FromMinutes(1L, 0L, 500L, 0L).Should().Be(TimeSpan.FromMilliseconds(60_500));
    }

    [Fact]
    public void FromHours_Test()
    {
        // Act & assert
        TimeSpan.FromHours(1, 30L, 0L, 0L, 0L).Should().Be(TimeSpan.FromMinutes(90));
    }

    [Fact]
    public void FromDays_Test()
    {
        // Act & assert
        TimeSpan.FromDays(1, 12, 0L, 0L, 0L, 0L).Should().Be(TimeSpan.FromHours(36));
    }
}
