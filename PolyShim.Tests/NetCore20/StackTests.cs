using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class StackTests
{
    [Fact]
    public void TryPeek_Test()
    {
        // Arrange
        var stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        // Act
        var result1 = stack.TryPeek(out var value1);
        var result2 = stack.TryPeek(out var value2);

        // Assert
        result1.Should().BeTrue();
        value1.Should().Be(3);
        result2.Should().BeTrue();
        value2.Should().Be(3);
        stack.Should().HaveCount(3);
    }

    [Fact]
    public void TryPeek_Empty_Test()
    {
        // Arrange
        var stack = new Stack<int>();

        // Act
        var result = stack.TryPeek(out var value);

        // Assert
        result.Should().BeFalse();
        value.Should().Be(0);
        stack.Should().BeEmpty();
    }

    [Fact]
    public void TryPeek_AfterPop_Test()
    {
        // Arrange
        var stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);

        // Act
        stack.TryPop(out _);
        var result = stack.TryPeek(out var value);

        // Assert
        result.Should().BeTrue();
        value.Should().Be(1);
        stack.Should().HaveCount(1);
    }

    [Fact]
    public void TryPop_Test()
    {
        // Arrange
        var stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        // Act
        var result1 = stack.TryPop(out var value1);
        var result2 = stack.TryPop(out var value2);
        var result3 = stack.TryPop(out var value3);

        // Assert
        result1.Should().BeTrue();
        value1.Should().Be(3);
        result2.Should().BeTrue();
        value2.Should().Be(2);
        result3.Should().BeTrue();
        value3.Should().Be(1);
        stack.Should().BeEmpty();
    }

    [Fact]
    public void TryPop_Empty_Test()
    {
        // Arrange
        var stack = new Stack<int>();

        // Act
        var result = stack.TryPop(out var value);

        // Assert
        result.Should().BeFalse();
        value.Should().Be(0);
        stack.Should().BeEmpty();
    }
}
