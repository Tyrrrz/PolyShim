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

        var act1 = () => ArgumentException.ThrowIfNullOrEmpty(emptyValue);
        var ex1 = act1.Should().Throw<ArgumentException>().Which;

        ex1.ParamName.Should().Be(nameof(emptyValue));

        var act2 = () => ArgumentException.ThrowIfNullOrEmpty(nullValue);
        var ex2 = act2.Should().Throw<ArgumentNullException>().Which;

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

        var act1 = () => ArgumentException.ThrowIfNullOrWhiteSpace(whiteSpaceValue);
        var ex1 = act1.Should().Throw<ArgumentException>().Which;

        ex1.ParamName.Should().Be(nameof(whiteSpaceValue));

        var act2 = () => ArgumentException.ThrowIfNullOrWhiteSpace(nullValue);
        var ex2 = act2.Should().Throw<ArgumentNullException>().Which;

        ex2.ParamName.Should().Be(nameof(nullValue));
    }
}
