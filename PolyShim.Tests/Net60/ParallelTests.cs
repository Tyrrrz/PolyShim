using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class ParallelTests
{
    [Fact]
    public async Task ForEachAsync_Test()
    {
        // Arrange
        var items = new[] { 'a', 'b', 'c', 'd', 'e' };
        var results = new ConcurrentBag<char>();

        // Act
        await Parallel.ForEachAsync(
            items,
            async (item, cancellationToken) =>
            {
                await Task.Delay(10, cancellationToken);
                results.Add(item);
            }
        );

        // Assert
        results.Should().BeEquivalentTo(items);
    }

    [Fact]
    public async Task ForEachAsync_Cancellation_Test()
    {
        // Arrange
        var items = new[] { 'a', 'b', 'c', 'd', 'e' };
        var cancellationToken = new CancellationToken(true);

        // Act & assert
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await Parallel.ForEachAsync(
                items,
                cancellationToken,
                async (_, innerCancellationToken) => await Task.Delay(10, innerCancellationToken)
            )
        );

        ex.CancellationToken.Should().Be(cancellationToken);
    }

    [Fact]
    public async Task ForEachAsync_MaxDegreeOfParallelism_Test()
    {
        // Arrange
        var items = new[] { 1, 2, 3, 4, 5 };
        var currentParallelism = 0;
        var maxObservedParallelism = 0;

        // Act
        await Parallel.ForEachAsync(
            items,
            new ParallelOptions { MaxDegreeOfParallelism = 2 },
            async (_, cancellationToken) =>
            {
                Interlocked.Increment(ref currentParallelism);

                int initialValue,
                    newValue;
                do
                {
                    initialValue = maxObservedParallelism;
                    newValue = Math.Max(initialValue, currentParallelism);
                } while (
                    Interlocked.CompareExchange(ref maxObservedParallelism, newValue, initialValue)
                    != initialValue
                );

                await Task.Delay(50, cancellationToken);
                Interlocked.Decrement(ref currentParallelism);
            }
        );

        // Assert
        maxObservedParallelism.Should().BeLessThanOrEqualTo(2);
    }

    [Fact]
    public async Task ForEachAsync_AsyncEnumerable_Test()
    {
        // Arrange
        async IAsyncEnumerable<int> GetItemsAsync()
        {
            for (var i = 1; i <= 5; i++)
            {
                await Task.Delay(10);
                yield return i;
            }
        }

        var results = new ConcurrentBag<int>();

        // Act
        await Parallel.ForEachAsync(
            GetItemsAsync(),
            async (item, cancellationToken) =>
            {
                await Task.Delay(10, cancellationToken);
                results.Add(item);
            }
        );

        // Assert
        results.Should().BeEquivalentTo([1, 2, 3, 4, 5]);
    }
}
