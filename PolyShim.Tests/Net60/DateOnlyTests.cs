using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class DateOnlyTests
{
    [Fact]
    public void Constructor_Test()
    {
        // Act
        var date = new DateOnly(2023, 5, 17);

        // Assert
        date.Year.Should().Be(2023);
        date.Month.Should().Be(5);
        date.Day.Should().Be(17);
    }

    [Fact]
    public void DayOfWeek_Test()
    {
        // Act
        var date = new DateOnly(2023, 5, 17);

        // Assert
        date.DayOfWeek.Should().Be(DayOfWeek.Wednesday);
    }

    [Fact]
    public void DayOfYear_Test()
    {
        // Act
        var date = new DateOnly(2023, 2, 1);

        // Assert
        date.DayOfYear.Should().Be(32);
    }

    [Fact]
    public void DayNumber_Test()
    {
        // Act
        var date = new DateOnly(1, 1, 1);

        // Assert
        date.DayNumber.Should().Be(0);
    }

    [Fact]
    public void AddDays_Test()
    {
        // Arrange
        var date = new DateOnly(2023, 5, 17);

        // Act
        var result = date.AddDays(10);

        // Assert
        result.Should().Be(new DateOnly(2023, 5, 27));
    }

    [Fact]
    public void AddMonths_Test()
    {
        // Arrange
        var date = new DateOnly(2023, 5, 17);

        // Act
        var result = date.AddMonths(2);

        // Assert
        result.Should().Be(new DateOnly(2023, 7, 17));
    }

    [Fact]
    public void AddYears_Test()
    {
        // Arrange
        var date = new DateOnly(2023, 5, 17);

        // Act
        var result = date.AddYears(1);

        // Assert
        result.Should().Be(new DateOnly(2024, 5, 17));
    }

    [Fact]
    public void FromDateTime_Test()
    {
        // Arrange
        var dateTime = new DateTime(2023, 5, 17, 13, 45, 0);

        // Act
        var date = DateOnly.FromDateTime(dateTime);

        // Assert
        date.Should().Be(new DateOnly(2023, 5, 17));
    }

    [Fact]
    public void FromDayNumber_Test()
    {
        // Act
        var date = DateOnly.FromDayNumber(0);

        // Assert
        date.Should().Be(new DateOnly(1, 1, 1));
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
    public void Deconstruct_Test()
    {
        // Arrange
        var date = new DateOnly(2023, 5, 17);

        // Act
        var (year, month, day) = date;

        // Assert
        year.Should().Be(2023);
        month.Should().Be(5);
        day.Should().Be(17);
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
    public void Operators_Test()
    {
        // Arrange
        var a = new DateOnly(2023, 5, 17);
        var b = new DateOnly(2023, 5, 18);

        // Act & assert
        (a == new DateOnly(2023, 5, 17))
            .Should()
            .BeTrue();
        (a != b).Should().BeTrue();
        (a < b).Should().BeTrue();
        (a <= b).Should().BeTrue();
        (b > a).Should().BeTrue();
        (b >= a).Should().BeTrue();
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
    public void TryParseExact_Test()
    {
        // Act
        var success = DateOnly.TryParseExact("17/05/2023", "dd/MM/yyyy", out var date);

        // Assert
        success.Should().BeTrue();
        date.Should().Be(new DateOnly(2023, 5, 17));
    }

    [Fact]
    public void MinValue_Test()
    {
        // Assert
        DateOnly.MinValue.Should().Be(new DateOnly(1, 1, 1));
    }

    [Fact]
    public void MaxValue_Test()
    {
        // Assert
        DateOnly.MaxValue.Should().Be(new DateOnly(9999, 12, 31));
    }
}
