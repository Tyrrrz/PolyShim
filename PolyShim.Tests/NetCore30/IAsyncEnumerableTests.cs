using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

file static class AsyncEnumerableHelpers
{
    public static async IAsyncEnumerable<int> GetNumbersAsync(
        int count,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        for (var i = 1; i <= count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Yield();
            yield return i;
        }
    }
}

public class AsyncEnumerableTests
{
    [Fact]
    public async Task AwaitForEach_Test()
    {
        // Arrange
        var result = new List<int>();

        // Act
        await foreach (var item in AsyncEnumerableHelpers.GetNumbersAsync(5))
            result.Add(item);

        // Assert
        result.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public async Task AwaitForEach_Empty_Test()
    {
        // Arrange
        var result = new List<int>();

        // Act
        await foreach (var item in AsyncEnumerableHelpers.GetNumbersAsync(0))
            result.Add(item);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AwaitForEach_WithCancellation_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var result = new List<int>();

        // Act
        await foreach (
            var item in AsyncEnumerableHelpers.GetNumbersAsync(5).WithCancellation(cts.Token)
        )
            result.Add(item);

        // Assert
        result.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public async Task AwaitForEach_WithCancellation_Canceled_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        // Act
        var act = async () =>
        {
            await foreach (
                var _ in AsyncEnumerableHelpers.GetNumbersAsync(5).WithCancellation(cts.Token)
            )
            {
                await cts.CancelAsync();
            }
        };

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
