using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class QueueTests
{
    [Fact]
    public void TryDequeue_WithElements_Test()
    {
        // Arrange
        var queue = new Queue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        var result1 = queue.TryDequeue(out var value1);
        var result2 = queue.TryDequeue(out var value2);
        var result3 = queue.TryDequeue(out var value3);

        // Assert
        result1.Should().BeTrue();
        value1.Should().Be(1);
        result2.Should().BeTrue();
        value2.Should().Be(2);
        result3.Should().BeTrue();
        value3.Should().Be(3);
        queue.Should().BeEmpty();
    }

    [Fact]
    public void TryDequeue_EmptyQueue_Test()
    {
        // Arrange
        var queue = new Queue<int>();

        // Act
        var result = queue.TryDequeue(out var value);

        // Assert
        result.Should().BeFalse();
        value.Should().Be(0);
        queue.Should().BeEmpty();
    }

    [Fact]
    public void TryPeek_WithElements_Test()
    {
        // Arrange
        var queue = new Queue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        var result1 = queue.TryPeek(out var value1);
        var result2 = queue.TryPeek(out var value2);

        // Assert
        result1.Should().BeTrue();
        value1.Should().Be(1);
        result2.Should().BeTrue();
        value2.Should().Be(1);
        queue.Should().HaveCount(3);
    }

    [Fact]
    public void TryPeek_EmptyQueue_Test()
    {
        // Arrange
        var queue = new Queue<int>();

        // Act
        var result = queue.TryPeek(out var value);

        // Assert
        result.Should().BeFalse();
        value.Should().Be(0);
        queue.Should().BeEmpty();
    }

    [Fact]
    public void TryPeek_AfterDequeue_Test()
    {
        // Arrange
        var queue = new Queue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);

        // Act
        queue.TryDequeue(out _);
        var result = queue.TryPeek(out var value);

        // Assert
        result.Should().BeTrue();
        value.Should().Be(2);
        queue.Should().HaveCount(1);
    }
}
