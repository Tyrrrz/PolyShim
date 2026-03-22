using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class ArgumentOutOfRangeExceptionTests
{
    [Fact]
    public void ThrowIfNegative_NonNegative_Test()
    {
        // Act & assert
        ArgumentOutOfRangeException.ThrowIfNegative(0);
        ArgumentOutOfRangeException.ThrowIfNegative(1);
        ArgumentOutOfRangeException.ThrowIfNegative(int.MaxValue);
    }

    [Fact]
    public void ThrowIfNegative_Negative_Test()
    {
        // Arrange
        const int negativeValue = -1;

        // Act
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArgumentOutOfRangeException.ThrowIfNegative(negativeValue)
        );

        // Assert
        ex.ParamName.Should().Be(nameof(negativeValue));
    }

    [Fact]
    public void ThrowIfNegativeOrZero_Positive_Test()
    {
        // Act & assert
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(1);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(int.MaxValue);
    }

    [Fact]
    public void ThrowIfNegativeOrZero_Zero_Test()
    {
        // Arrange
        const int zeroValue = 0;

        // Act
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(zeroValue)
        );

        // Assert
        ex.ParamName.Should().Be(nameof(zeroValue));
    }

    [Fact]
    public void ThrowIfNegativeOrZero_Negative_Test()
    {
        // Arrange
        const int negativeValue = -5;

        // Act
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(negativeValue)
        );

        // Assert
        ex.ParamName.Should().Be(nameof(negativeValue));
    }

    [Fact]
    public void ThrowIfZero_NonZero_Test()
    {
        // Act & assert
        ArgumentOutOfRangeException.ThrowIfZero(1);
        ArgumentOutOfRangeException.ThrowIfZero(-1);
    }

    [Fact]
    public void ThrowIfZero_Zero_Test()
    {
        // Arrange
        const int zeroValue = 0;

        // Act
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArgumentOutOfRangeException.ThrowIfZero(zeroValue)
        );

        // Assert
        ex.ParamName.Should().Be(nameof(zeroValue));
    }

    [Fact]
    public void ThrowIfGreaterThan_WithinRange_Test()
    {
        // Act & assert
        ArgumentOutOfRangeException.ThrowIfGreaterThan(5, 10);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(10, 10);
    }

    [Fact]
    public void ThrowIfGreaterThan_ExceedsMax_Test()
    {
        // Arrange
        const int value = 11;

        // Act
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 10)
        );

        // Assert
        ex.ParamName.Should().Be(nameof(value));
    }

    [Fact]
    public void ThrowIfGreaterThanOrEqual_WithinRange_Test()
    {
        // Act & assert
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(5, 10);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(-1, 0);
    }

    [Fact]
    public void ThrowIfGreaterThanOrEqual_Equal_Test()
    {
        // Arrange
        const int value = 10;

        // Act
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, 10)
        );

        // Assert
        ex.ParamName.Should().Be(nameof(value));
    }

    [Fact]
    public void ThrowIfLessThan_WithinRange_Test()
    {
        // Act & assert
        ArgumentOutOfRangeException.ThrowIfLessThan(10, 5);
        ArgumentOutOfRangeException.ThrowIfLessThan(5, 5);
    }

    [Fact]
    public void ThrowIfLessThan_BelowMin_Test()
    {
        // Arrange
        const int value = 4;

        // Act
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 5)
        );

        // Assert
        ex.ParamName.Should().Be(nameof(value));
    }

    [Fact]
    public void ThrowIfLessThanOrEqual_WithinRange_Test()
    {
        // Act & assert
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(10, 5);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(6, 5);
    }

    [Fact]
    public void ThrowIfLessThanOrEqual_Equal_Test()
    {
        // Arrange
        const int value = 5;

        // Act
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, 5)
        );

        // Assert
        ex.ParamName.Should().Be(nameof(value));
    }

    [Fact]
    public void ThrowIfEqual_NotEqual_Test()
    {
        // Act & assert
        ArgumentOutOfRangeException.ThrowIfEqual(1, 2);
        ArgumentOutOfRangeException.ThrowIfEqual("hello", "world");
    }

    [Fact]
    public void ThrowIfEqual_Equal_Test()
    {
        // Arrange
        const int value = 42;

        // Act
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArgumentOutOfRangeException.ThrowIfEqual(value, 42)
        );

        // Assert
        ex.ParamName.Should().Be(nameof(value));
    }

    [Fact]
    public void ThrowIfNotEqual_Equal_Test()
    {
        // Act & assert
        ArgumentOutOfRangeException.ThrowIfNotEqual(42, 42);
        ArgumentOutOfRangeException.ThrowIfNotEqual("hello", "hello");
    }

    [Fact]
    public void ThrowIfNotEqual_NotEqual_Test()
    {
        // Arrange
        const int value = 42;

        // Act
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ArgumentOutOfRangeException.ThrowIfNotEqual(value, 100)
        );

        // Assert
        ex.ParamName.Should().Be(nameof(value));
    }
}
