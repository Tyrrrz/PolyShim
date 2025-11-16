using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class ArgumentExceptionTests
{
    [Fact]
    public void ThrowIfNullOrEmpty_Test()
    {
        // Arrange
        const string notEmptyValue = "Hello, World!";
        const string emptyValue = "";
        const string? nullValue = null;

        // Act & assert
        ArgumentException.ThrowIfNullOrEmpty(notEmptyValue);

        var ex1 = Assert.Throws<ArgumentException>(() =>
            ArgumentException.ThrowIfNullOrEmpty(emptyValue)
        );

        ex1.ParamName.Should().Be(nameof(emptyValue));

        var ex2 = Assert.Throws<ArgumentNullException>(() =>
            ArgumentException.ThrowIfNullOrEmpty(nullValue)
        );

        ex2.ParamName.Should().Be(nameof(nullValue));
    }

    [Fact]
    public void ThrowIfNullOrWhiteSpace_Test()
    {
        // Arrange
        const string notWhiteSpaceValue = "Hello, World!";
        const string whiteSpaceValue = "   ";
        const string? nullValue = null;

        // Act & assert
        ArgumentException.ThrowIfNullOrWhiteSpace(notWhiteSpaceValue);

        var ex1 = Assert.Throws<ArgumentException>(() =>
            ArgumentException.ThrowIfNullOrWhiteSpace(whiteSpaceValue)
        );

        ex1.ParamName.Should().Be(nameof(whiteSpaceValue));

        var ex2 = Assert.Throws<ArgumentNullException>(() =>
            ArgumentException.ThrowIfNullOrWhiteSpace(nullValue)
        );

        ex2.ParamName.Should().Be(nameof(nullValue));
    }
}
