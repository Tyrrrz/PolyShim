using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class StringTests
{
    [Fact]
    public void IndexOf_Char_Ordinal_Test()
    {
        // Act & assert
        "Hello, World!".IndexOf('o', StringComparison.Ordinal).Should().Be(4);
        "Hello, World!".IndexOf('z', StringComparison.Ordinal).Should().Be(-1);
    }

    [Fact]
    public void IndexOf_Char_OrdinalIgnoreCase_Test()
    {
        // Act & assert
        "Hello, World!".IndexOf('O', StringComparison.OrdinalIgnoreCase).Should().Be(4);
        "Hello, World!".IndexOf('Z', StringComparison.OrdinalIgnoreCase).Should().Be(-1);
    }

    [Fact]
    public void IndexOf_Char_CurrentCulture_Test()
    {
        // Act & assert
        "Hello, World!".IndexOf('W', StringComparison.CurrentCulture).Should().Be(7);
    }
}
