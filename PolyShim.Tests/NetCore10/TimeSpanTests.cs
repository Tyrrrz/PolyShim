using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class TimeSpanTests
{
    [Fact]
    public void Parse_WithFormatProvider_Test()
    {
        // Act & assert
        TimeSpan.Parse("1:30:00", null).Should().Be(TimeSpan.FromMinutes(90));
        TimeSpan
            .Parse("1.12:30:45", CultureInfo.InvariantCulture)
            .Should()
            .Be(new TimeSpan(1, 12, 30, 45));
        TimeSpan
            .Parse("-2:00:00", CultureInfo.InvariantCulture)
            .Should()
            .Be(TimeSpan.FromHours(-2));
    }

    [Fact]
    public void TryParse_WithFormatProvider_ValidInput_Test()
    {
        // Act
        var success = TimeSpan.TryParse("1:30:00", null, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().Be(TimeSpan.FromMinutes(90));
    }

    [Fact]
    public void TryParse_WithFormatProvider_InvalidInput_Test()
    {
        // Act
        var success = TimeSpan.TryParse(
            "not-a-timespan",
            CultureInfo.InvariantCulture,
            out var result
        );

        // Assert
        success.Should().BeFalse();
        result.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public void TryParse_WithFormatProvider_NullInput_Test()
    {
        // Act
        var success = TimeSpan.TryParse(null, CultureInfo.InvariantCulture, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public void ParseExact_ConstantFormat_Test()
    {
        // Act & assert
        TimeSpan.ParseExact("01:30:00", "c", null).Should().Be(TimeSpan.FromMinutes(90));
        TimeSpan.ParseExact("1.02:03:04", "c", null).Should().Be(new TimeSpan(1, 2, 3, 4));
        TimeSpan.ParseExact("-01:30:00", "c", null).Should().Be(TimeSpan.FromMinutes(-90));
        TimeSpan
            .ParseExact("01:30:00.1234567", "c", null)
            .Should()
            .Be(new TimeSpan(0, 1, 30, 0, 123) + TimeSpan.FromTicks(4567));
    }

    [Fact]
    public void ParseExact_GeneralShortFormat_Test()
    {
        // Act & assert
        TimeSpan.ParseExact("1:30:00", "g", null).Should().Be(TimeSpan.FromMinutes(90));
        TimeSpan.ParseExact("1:2:30:00", "g", null).Should().Be(new TimeSpan(1, 2, 30, 0));
        TimeSpan.ParseExact("-1:30:00", "g", null).Should().Be(TimeSpan.FromMinutes(-90));
    }

    [Fact]
    public void ParseExact_GeneralLongFormat_Test()
    {
        // Act & assert
        TimeSpan.ParseExact("1:02:03:04.0000000", "G", null).Should().Be(new TimeSpan(1, 2, 3, 4));
        TimeSpan
            .ParseExact("-1:02:03:04.0000000", "G", null)
            .Should()
            .Be(-new TimeSpan(1, 2, 3, 4));
    }

    [Fact]
    public void ParseExact_MultipleFormats_Test()
    {
        // Act & assert
        TimeSpan
            .ParseExact("1:30:00", new[] { "g", "c" }, null)
            .Should()
            .Be(TimeSpan.FromMinutes(90));
        TimeSpan
            .ParseExact("01:30:00", new[] { "g", "c" }, null)
            .Should()
            .Be(TimeSpan.FromMinutes(90));
    }

    [Fact]
    public void ParseExact_InvalidInput_ThrowsFormatException_Test()
    {
        // Act & assert
        var act = () => TimeSpan.ParseExact("not-a-timespan", "c", null);
        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void ParseExact_WithStyles_AssumeNegative_Test()
    {
        // AssumeNegative only applies to custom format strings without a sign specifier
        TimeSpan
            .ParseExact("1:30", @"h\:mm", null, TimeSpanStyles.AssumeNegative)
            .Should()
            .Be(TimeSpan.FromMinutes(-90));
    }

    [Fact]
    public void ParseExact_WithStyles_MultipleFormats_Test()
    {
        // Act & assert
        TimeSpan
            .ParseExact("1:30:00", new[] { "g", "c" }, null, TimeSpanStyles.None)
            .Should()
            .Be(TimeSpan.FromMinutes(90));
    }

    [Fact]
    public void TryParseExact_ConstantFormat_ValidInput_Test()
    {
        // Act
        var success = TimeSpan.TryParseExact("01:30:00", "c", null, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().Be(TimeSpan.FromMinutes(90));
    }

    [Fact]
    public void TryParseExact_ConstantFormat_InvalidInput_Test()
    {
        // Act
        var success = TimeSpan.TryParseExact("not-valid", "c", null, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public void TryParseExact_MultipleFormats_Test()
    {
        // Act
        var success = TimeSpan.TryParseExact("1:30:00", new[] { "g", "c" }, null, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().Be(TimeSpan.FromMinutes(90));
    }

    [Fact]
    public void TryParseExact_WithStyles_AssumeNegative_Test()
    {
        // AssumeNegative only applies to custom format strings without a sign specifier
        var success = TimeSpan.TryParseExact(
            "1:30",
            @"h\:mm",
            null,
            TimeSpanStyles.AssumeNegative,
            out var result
        );

        // Assert
        success.Should().BeTrue();
        result.Should().Be(TimeSpan.FromMinutes(-90));
    }

    [Fact]
    public void TryParseExact_WithStyles_MultipleFormats_Test()
    {
        // Act
        var success = TimeSpan.TryParseExact(
            "1:30:00",
            new[] { "g", "c" },
            null,
            TimeSpanStyles.None,
            out var result
        );

        // Assert
        success.Should().BeTrue();
        result.Should().Be(TimeSpan.FromMinutes(90));
    }

    [Fact]
    public void TryParseExact_NullFormats_Test()
    {
        // Act
        var success = TimeSpan.TryParseExact("01:30:00", (string[]?)null, null, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public void TryParseExact_CustomFormat_Test()
    {
        // Act & assert
        TimeSpan.TryParseExact("1:30", @"h\:mm", null, out var result).Should().BeTrue();
        result.Should().Be(new TimeSpan(1, 30, 0));
    }
}
