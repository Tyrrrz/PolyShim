using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class TimeOnlyTests
{
    [Fact]
    public void Deconstruct_HourMinute_Test()
    {
        // Arrange
        var time = new TimeOnly(13, 45, 30);

        // Act
        var (hour, minute) = time;

        // Assert
        hour.Should().Be(13);
        minute.Should().Be(45);
    }

    [Fact]
    public void Deconstruct_HourMinuteSecond_Test()
    {
        // Arrange
        var time = new TimeOnly(13, 45, 30);

        // Act
        var (hour, minute, second) = time;

        // Assert
        hour.Should().Be(13);
        minute.Should().Be(45);
        second.Should().Be(30);
    }

    [Fact]
    public void Deconstruct_HourMinuteSecondMillisecond_Test()
    {
        // Arrange
        var time = new TimeOnly(13, 45, 30, 500);

        // Act
        var (hour, minute, second, millisecond) = time;

        // Assert
        hour.Should().Be(13);
        minute.Should().Be(45);
        second.Should().Be(30);
        millisecond.Should().Be(500);
    }
}
