using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class QueueTests
{
    [Fact]
    public void EnsureCapacity_HigherCapacity_Test()
    {
        // Arrange
        var queue = new Queue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        var newCapacity = queue.EnsureCapacity(100);

        // Assert
        newCapacity.Should().BeGreaterThanOrEqualTo(100);
        queue.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void EnsureCapacity_NegativeCapacity_Test()
    {
        // Arrange
        var queue = new Queue<int>();

        // Act
        var act = () => queue.EnsureCapacity(-1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
