using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class StackTests
{
    [Fact]
    public void EnsureCapacity_HigherCapacity_Test()
    {
        // Arrange
        var stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        // Act
        var newCapacity = stack.EnsureCapacity(100);

        // Assert
        newCapacity.Should().Be(100);
        stack.Should().Equal(3, 2, 1);
    }

    [Fact]
    public void EnsureCapacity_NegativeCapacity_Test()
    {
        // Arrange
        var stack = new Stack<int>();

        // Act
        var act = () => stack.EnsureCapacity(-1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
