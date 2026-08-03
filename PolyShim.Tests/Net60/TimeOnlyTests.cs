using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class TimeOnlyTests
{
    [Fact]
    public void Constructor_Test()
    {
        // Act
        var time = new TimeOnly(13, 45, 30, 500);

        // Assert
        time.Hour.Should().Be(13);
        time.Minute.Should().Be(45);
        time.Second.Should().Be(30);
        time.Millisecond.Should().Be(500);
    }

    [Fact]
    public void Add_Test()
    {
        // Arrange
        var time = new TimeOnly(13, 45, 0);

        // Act
        var result = time.Add(TimeSpan.FromHours(2));

        // Assert
        result.Should().Be(new TimeOnly(15, 45, 0));
    }

    [Fact]
    public void Add_WrapsAroundMidnight_Test()
    {
        // Arrange
        var time = new TimeOnly(23, 0, 0);

        // Act
        var result = time.Add(TimeSpan.FromHours(2), out var wrappedDays);

        // Assert
        result.Should().Be(new TimeOnly(1, 0, 0));
        wrappedDays.Should().Be(1);
    }

    [Fact]
    public void AddHours_Test()
    {
        // Arrange
        var time = new TimeOnly(13, 45, 0);

        // Act
        var result = time.AddHours(1.5);

        // Assert
        result.Should().Be(new TimeOnly(15, 15, 0));
    }

    [Fact]
    public void AddMinutes_Test()
    {
        // Arrange
        var time = new TimeOnly(13, 45, 0);

        // Act
        var result = time.AddMinutes(30);

        // Assert
        result.Should().Be(new TimeOnly(14, 15, 0));
    }

    [Fact]
    public void IsBetween_Test()
    {
        // Arrange
        var time = new TimeOnly(11, 0, 0);
        var start = new TimeOnly(10, 0, 0);
        var end = new TimeOnly(12, 0, 0);

        // Act & assert
        time.IsBetween(start, end).Should().BeTrue();
        start.IsBetween(start, end).Should().BeTrue();
        end.IsBetween(start, end).Should().BeFalse();
    }

    [Fact]
    public void IsBetween_WrapsAroundMidnight_Test()
    {
        // Arrange
        var start = new TimeOnly(23, 0, 0);
        var end = new TimeOnly(1, 0, 0);

        // Act & assert
        new TimeOnly(0, 0, 0)
            .IsBetween(start, end)
            .Should()
            .BeTrue();
        new TimeOnly(12, 0, 0).IsBetween(start, end).Should().BeFalse();
    }

    [Fact]
    public void FromDateTime_Test()
    {
        // Arrange
        var dateTime = new DateTime(2023, 5, 17, 13, 45, 30);

        // Act
        var time = TimeOnly.FromDateTime(dateTime);

        // Assert
        time.Should().Be(new TimeOnly(13, 45, 30));
    }

    [Fact]
    public void FromTimeSpan_Test()
    {
        // Act
        var time = TimeOnly.FromTimeSpan(new TimeSpan(13, 45, 30));

        // Assert
        time.Should().Be(new TimeOnly(13, 45, 30));
    }

    [Fact]
    public void ToTimeSpan_Test()
    {
        // Arrange
        var time = new TimeOnly(13, 45, 30);

        // Act
        var timeSpan = time.ToTimeSpan();

        // Assert
        timeSpan.Should().Be(new TimeSpan(13, 45, 30));
    }

    [Fact]
    public void CompareTo_Test()
    {
        // Arrange
        var a = new TimeOnly(10, 0, 0);
        var b = new TimeOnly(11, 0, 0);

        // Act & assert
        a.CompareTo(b).Should().BeNegative();
        b.CompareTo(a).Should().BePositive();
        a.CompareTo(a).Should().Be(0);
    }

    [Fact]
    public void Equals_Test()
    {
        // Arrange
        var a = new TimeOnly(10, 0, 0);
        var b = new TimeOnly(10, 0, 0);
        var c = new TimeOnly(11, 0, 0);

        // Act & assert
        a.Equals(b).Should().BeTrue();
        a.Equals(c).Should().BeFalse();
        a.Equals((object)b).Should().BeTrue();
        a.Equals((object)c).Should().BeFalse();
        a.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_Test()
    {
        // Arrange
        var a = new TimeOnly(10, 0, 0);
        var b = new TimeOnly(10, 0, 0);
        var c = new TimeOnly(11, 0, 0);

        // Act & assert
        a.GetHashCode().Should().Be(b.GetHashCode());
        a.GetHashCode().Should().NotBe(c.GetHashCode());
    }

    [Fact]
    public void Operators_Test()
    {
        // Arrange
        var a = new TimeOnly(10, 0, 0);
        var b = new TimeOnly(11, 0, 0);

        // Act & assert
        (a == new TimeOnly(10, 0, 0))
            .Should()
            .BeTrue();
        (a != b).Should().BeTrue();
        (a < b).Should().BeTrue();
        (a <= b).Should().BeTrue();
        (b > a).Should().BeTrue();
        (b >= a).Should().BeTrue();
        (b - a).Should().Be(TimeSpan.FromHours(1));
    }

    [Fact]
    public void SubtractionOperator_WrapsAroundMidnight_Test()
    {
        // Arrange
        var a = new TimeOnly(23, 0, 0);
        var b = new TimeOnly(1, 0, 0);

        // Act & assert
        (b - a)
            .Should()
            .Be(TimeSpan.FromHours(2));
    }

    [Fact]
    public void ToString_Test()
    {
        // Arrange
        var time = new TimeOnly(13, 45, 30);

        // Act & assert
        time.ToString("HH:mm:ss", CultureInfo.InvariantCulture).Should().Be("13:45:30");
    }

    [Fact]
    public void Parse_Test()
    {
        // Act
        var time = TimeOnly.Parse("13:45:30", CultureInfo.InvariantCulture);

        // Assert
        time.Should().Be(new TimeOnly(13, 45, 30));
    }

    [Fact]
    public void ParseExact_Test()
    {
        // Act
        var time = TimeOnly.ParseExact("13-45-30", "HH-mm-ss", CultureInfo.InvariantCulture);

        // Assert
        time.Should().Be(new TimeOnly(13, 45, 30));
    }

    [Fact]
    public void TryParse_Test()
    {
        // Act
        var success = TimeOnly.TryParse(
            "13:45:30",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var time
        );

        // Assert
        success.Should().BeTrue();
        time.Should().Be(new TimeOnly(13, 45, 30));
    }

    [Fact]
    public void TryParse_Failure_Test()
    {
        // Act
        var success = TimeOnly.TryParse("not a time", out _);

        // Assert
        success.Should().BeFalse();
    }

    [Fact]
    public void TryParse_WithDateComponent_Failure_Test()
    {
        // Arrange & Act
        var success = TimeOnly.TryParse("2023-05-17 13:45:30", out _);

        // Assert
        success.Should().BeFalse();
    }

    [Fact]
    public void TryParseExact_Test()
    {
        // Act
        var success = TimeOnly.TryParseExact("13-45-30", "HH-mm-ss", out var time);

        // Assert
        success.Should().BeTrue();
        time.Should().Be(new TimeOnly(13, 45, 30));
    }

    [Fact]
    public void MinValue_Test()
    {
        // Assert
        DateOnly.MinValue.DayNumber.Should().Be(0);
        TimeOnly.MinValue.Should().Be(new TimeOnly(0, 0, 0));
    }

    [Fact]
    public void MaxValue_Test()
    {
        // Assert
        TimeOnly.MaxValue.Ticks.Should().Be(TimeSpan.TicksPerDay - 1);
    }
}
