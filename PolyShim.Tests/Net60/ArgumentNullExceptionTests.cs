using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class ArgumentNullExceptionTests
{
    [Fact]
    public void ThrowIfNull_Test()
    {
        // Arrange
        const string notNullValue = "Hello, World!";
        const string? nullValue = null;

        // Act & assert
        ArgumentNullException.ThrowIfNull(notNullValue);

        var act = () => ArgumentNullException.ThrowIfNull(nullValue);
        var ex = act.Should().Throw<ArgumentNullException>().Which;

        ex.ParamName.Should().Be(nameof(nullValue));
    }
}
