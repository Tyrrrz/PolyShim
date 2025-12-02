using System;
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
        var results = new List<char>();

        // Act
        await Parallel.ForEachAsync(
            items,
            async (item, cancellationToken) =>
            {
                await Task.Delay(10, cancellationToken);

                lock (results)
                {
                    results.Add(item);
                }
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
}
