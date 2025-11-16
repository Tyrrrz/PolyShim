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

        var ex = Assert.Throws<ArgumentNullException>(() =>
            ArgumentNullException.ThrowIfNull(nullValue)
        );

        ex.ParamName.Should().Be(nameof(nullValue));
    }
}
