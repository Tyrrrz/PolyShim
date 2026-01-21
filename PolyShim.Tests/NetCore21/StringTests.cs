using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class StringTests
{
    [Fact]
    public void Contains_Char_Test()
    {
        // Arrange
        const string str = "abc";

        // Act & assert
        str.Contains('B', StringComparison.OrdinalIgnoreCase).Should().BeTrue();
        str.Contains('B', StringComparison.Ordinal).Should().BeFalse();
    }

    [Fact]
    public void Contains_String_Test()
    {
        // Arrange
        const string str = "abc";

        // Act & assert
        str.Contains("B", StringComparison.OrdinalIgnoreCase).Should().BeTrue();
        str.Contains("B", StringComparison.Ordinal).Should().BeFalse();
    }

    [Fact]
    public void AsSpan_WithStartAndLength_Test()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var span = text.AsSpan(0, 5);

        // Assert
        span.ToArray().Should().Equal('H', 'e', 'l', 'l', 'o');
    }

    [Fact]
    public void AsSpan_WithStart_Test()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var span = text.AsSpan(6);

        // Assert
        span.ToArray().Should().Equal('W', 'o', 'r', 'l', 'd');
    }

    [Fact]
    public void AsSpan_Default_Test()
    {
        // Arrange
        var text = "Hello";

        // Act
        var span = text.AsSpan();

        // Assert
        span.ToArray().Should().Equal('H', 'e', 'l', 'l', 'o');
    }

    [Fact]
    public void AsMemory_WithStartAndLength_Test()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var memory = text.AsMemory(0, 5);

        // Assert
        memory.ToArray().Should().Equal('H', 'e', 'l', 'l', 'o');
    }

    [Fact]
    public void AsMemory_WithStart_Test()
    {
        // Arrange
        var text = "Hello World";

        // Act
        var memory = text.AsMemory(6);

        // Assert
        memory.ToArray().Should().Equal('W', 'o', 'r', 'l', 'd');
    }

    [Fact]
    public void AsMemory_Default_Test()
    {
        // Arrange
        var text = "Hello";

        // Act
        var memory = text.AsMemory();

        // Assert
        memory.ToArray().Should().Equal('H', 'e', 'l', 'l', 'o');
    }
}
