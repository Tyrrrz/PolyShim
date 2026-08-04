using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class DateOnlyTests
{
    [Fact]
    public void Constants_Test()
    {
        DateOnly.MinValue.Should().Be(new DateOnly(1, 1, 1));
        DateOnly.MaxValue.Should().Be(new DateOnly(9999, 12, 31));
    }

    [Fact]
    public void Constructor_Test()
    {
        // Act
        var date = new DateOnly(2023, 5, 17);

        // Assert
        date.Year.Should().Be(2023);
        date.Month.Should().Be(5);
        date.Day.Should().Be(17);
        date.DayOfWeek.Should().Be(DayOfWeek.Wednesday);
        new DateOnly(2023, 2, 1).DayOfYear.Should().Be(32);
        new DateOnly(1, 1, 1).DayNumber.Should().Be(0);
    }

    [Fact]
    public void Add_Test()
    {
        // Arrange
        var date = new DateOnly(2023, 5, 17);

        // Act & assert
        date.AddDays(10).Should().Be(new DateOnly(2023, 5, 27));
        date.AddMonths(2).Should().Be(new DateOnly(2023, 7, 17));
        date.AddYears(1).Should().Be(new DateOnly(2024, 5, 17));
    }

    [Fact]
    public void From_Test()
    {
        // Act & assert
        DateOnly
            .FromDateTime(new DateTime(2023, 5, 17, 13, 45, 0))
            .Should()
            .Be(new DateOnly(2023, 5, 17));
        DateOnly.FromDayNumber(0).Should().Be(new DateOnly(1, 1, 1));
    }

    [Fact]
    public void ToDateTime_Test()
    {
        // Arrange
        var date = new DateOnly(2023, 5, 17);
        var time = new TimeOnly(13, 45, 30);

        // Act
        var dateTime = date.ToDateTime(time);

        // Assert
        dateTime.Should().Be(new DateTime(2023, 5, 17, 13, 45, 30));
    }

    [Fact]
    public void CompareTo_Test()
    {
        // Arrange
        var a = new DateOnly(2023, 5, 17);
        var b = new DateOnly(2023, 5, 18);

        // Act & assert
        a.CompareTo(b).Should().BeNegative();
        b.CompareTo(a).Should().BePositive();
        a.CompareTo(a).Should().Be(0);
    }

    [Fact]
    public void Equals_Test()
    {
        // Arrange
        var a = new DateOnly(2023, 5, 17);
        var b = new DateOnly(2023, 5, 17);
        var c = new DateOnly(2023, 5, 18);

        // Act & assert
        a.Equals(b).Should().BeTrue();
        a.Equals(c).Should().BeFalse();
        a.Equals((object)b).Should().BeTrue();
        a.Equals((object)c).Should().BeFalse();
        a.Equals(null).Should().BeFalse();
        (a == b).Should().BeTrue();
        (a != c).Should().BeTrue();
        (a < c).Should().BeTrue();
        (a <= c).Should().BeTrue();
        (c > a).Should().BeTrue();
        (c >= a).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_Test()
    {
        // Arrange
        var a = new DateOnly(2023, 5, 17);
        var b = new DateOnly(2023, 5, 17);
        var c = new DateOnly(2023, 5, 18);

        // Act & assert
        a.GetHashCode().Should().Be(b.GetHashCode());
        a.GetHashCode().Should().NotBe(c.GetHashCode());
    }

    [Fact]
    public void ToString_Test()
    {
        // Arrange
        var date = new DateOnly(2023, 5, 17);

        // Act & assert
        date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture).Should().Be("2023-05-17");
    }

    [Fact]
    public void Parse_Test()
    {
        // Act
        var date = DateOnly.Parse("2023-05-17", CultureInfo.InvariantCulture);

        // Assert
        date.Should().Be(new DateOnly(2023, 5, 17));
    }

    [Fact]
    public void ParseExact_Test()
    {
        // Act
        var date = DateOnly.ParseExact("17/05/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);

        // Assert
        date.Should().Be(new DateOnly(2023, 5, 17));
    }

    [Fact]
    public void TryParse_Test()
    {
        // Act
        var success = DateOnly.TryParse(
            "2023-05-17",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var date
        );

        // Assert
        success.Should().BeTrue();
        date.Should().Be(new DateOnly(2023, 5, 17));
    }

    [Fact]
    public void TryParse_Failure_Test()
    {
        // Act
        var success = DateOnly.TryParse("not a date", out _);

        // Assert
        success.Should().BeFalse();
    }

    [Fact]
    public void TryParse_WithTimeComponent_Failure_Test()
    {
        // Arrange & Act
        var success = DateOnly.TryParse("2023-05-17 13:45:30", out _);

        // Assert
        success.Should().BeFalse();
    }

    [Fact]
    public void TryParseExact_Test()
    {
        // Act
        var success = DateOnly.TryParseExact("17/05/2023", "dd/MM/yyyy", out var date);

        // Assert
        success.Should().BeTrue();
        date.Should().Be(new DateOnly(2023, 5, 17));
    }
}
