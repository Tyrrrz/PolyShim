using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class BlockingCollectionTests
{
    [Fact]
    public void AddAndTake_Test()
    {
        // Arrange
        using var collection = new BlockingCollection<int>();

        // Act
        collection.Add(1);
        collection.Add(2);

        // Assert
        collection.Count.Should().Be(2);
        collection.Take().Should().Be(1);
        collection.Take().Should().Be(2);
    }

    [Fact]
    public void TryTake_Test()
    {
        // Arrange
        using var collection = new BlockingCollection<int>();
        collection.Add(1);

        // Act & assert
        collection.TryTake(out var item).Should().BeTrue();
        item.Should().Be(1);

        collection.TryTake(out _).Should().BeFalse();
    }

    [Fact]
    public void TryTake_Timeout_Test()
    {
        // Arrange
        using var collection = new BlockingCollection<int>();

        // Act & assert
        collection.TryTake(out _, 100).Should().BeFalse();
        collection.TryTake(out _, TimeSpan.FromMilliseconds(100)).Should().BeFalse();
        collection.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public void TryTake_InvalidTimeout_Test()
    {
        // Arrange
        using var collection = new BlockingCollection<int>();

        // Act & assert
        var act = () => collection.TryTake(out _, -2);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void TryTake_Blocking_Test()
    {
        // Arrange
        using var collection = new BlockingCollection<int>();

        var thread = new Thread(() =>
        {
            Thread.Sleep(200);
            collection.Add(42);
        });

        // Act
        thread.Start();
        var taken = collection.TryTake(out var item, Timeout.Infinite);
        thread.Join();

        // Assert
        taken.Should().BeTrue();
        item.Should().Be(42);
    }

    [Fact]
    public void CompleteAdding_Test()
    {
        // Arrange
        using var collection = new BlockingCollection<int>();
        collection.Add(1);

        // Act
        collection.CompleteAdding();

        // Assert
        collection.IsAddingCompleted.Should().BeTrue();
        collection.IsCompleted.Should().BeFalse();

        collection.Take().Should().Be(1);
        collection.IsCompleted.Should().BeTrue();

        collection.TryTake(out _, Timeout.Infinite).Should().BeFalse();

        var add = () => collection.Add(2);
        add.Should().Throw<InvalidOperationException>();

        var take = () => collection.Take();
        take.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void CompleteAdding_Wakes_Test()
    {
        // Arrange
        using var collection = new BlockingCollection<int>();

        var thread = new Thread(() =>
        {
            Thread.Sleep(200);
            collection.CompleteAdding();
        });

        // Act
        thread.Start();
        var taken = collection.TryTake(out _, Timeout.Infinite);
        thread.Join();

        // Assert
        taken.Should().BeFalse();
        collection.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public void GetConsumingEnumerable_Test()
    {
        // Arrange
        using var collection = new BlockingCollection<int>();

        var thread = new Thread(() =>
        {
            for (var i = 0; i < 5; i++)
                collection.Add(i);

            collection.CompleteAdding();
        });

        // Act
        thread.Start();
        var items = collection.GetConsumingEnumerable().ToArray();
        thread.Join();

        // Assert
        items.Should().Equal(0, 1, 2, 3, 4);
    }
}
