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
        TimeSpan.FromMilliseconds(1234, 567).Should().Be(TimeSpan.FromSeconds(1.234567));
        TimeSpan.FromMilliseconds(0, 0).Should().Be(TimeSpan.Zero);
        TimeSpan.FromMilliseconds(-1000, -500).Should().Be(TimeSpan.FromSeconds(-1.0005));
    }

    [Fact]
    public void FromSeconds_Test()
    {
        // Act & assert
        TimeSpan.FromSeconds(10).Should().Be(TimeSpan.FromSeconds(10));
        TimeSpan.FromSeconds(1, 500).Should().Be(TimeSpan.FromMilliseconds(1500));
        TimeSpan.FromSeconds(1, 500, 250).Should().Be(TimeSpan.FromMilliseconds(1500.25));
    }

    [Fact]
    public void FromMinutes_Test()
    {
        // Act & assert
        TimeSpan.FromMinutes(2).Should().Be(TimeSpan.FromMinutes(2));
        TimeSpan.FromMinutes(1, 30).Should().Be(TimeSpan.FromSeconds(90));
        TimeSpan.FromMinutes(1, 0, 500).Should().Be(TimeSpan.FromMilliseconds(60_500));
    }

    [Fact]
    public void FromHours_Test()
    {
        // Act & assert
        TimeSpan.FromHours(2).Should().Be(TimeSpan.FromHours(2));
        TimeSpan.FromHours(1, 30).Should().Be(TimeSpan.FromMinutes(90));
    }

    [Fact]
    public void FromDays_Test()
    {
        // Act & assert
        TimeSpan.FromDays(2).Should().Be(TimeSpan.FromDays(2));
        TimeSpan.FromDays(1, 12).Should().Be(TimeSpan.FromHours(36));
    }
}
