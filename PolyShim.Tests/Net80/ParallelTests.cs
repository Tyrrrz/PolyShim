using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class ParallelTests
{
    [Fact]
    public async Task ForAsync_Test()
    {
        // Act
        var sum = 0;
        await Parallel.ForAsync(
            1,
            6,
            async (i, cancellationToken) =>
            {
                await Task.Delay(10, cancellationToken);
                Interlocked.Add(ref sum, i);
            }
        );

        // Assert
        sum.Should().Be(15);
    }

    [Fact]
    public async Task ForAsync_Cancellation_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);

        // Act & assert
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await Parallel.ForAsync(
                1,
                6,
                cancellationToken,
                async (_, innerCancellationToken) =>
                {
                    await Task.Delay(10, innerCancellationToken);
                }
            )
        );

        ex.CancellationToken.Should().Be(cancellationToken);
    }
}
