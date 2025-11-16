using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class TaskCompletionSourceTests
{
    [Fact]
    public async Task SetCanceled_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var tcs = new TaskCompletionSource<int>();

        // Act
        tcs.SetCanceled(cancellationToken);
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await tcs.Task
        );

        // Assert
        tcs.Task.IsCanceled.Should().BeTrue();
        ex.CancellationToken.Should().Be(cancellationToken);
        ex.CancellationToken.IsCancellationRequested.Should().BeTrue();
    }

    [Fact]
    public void SetCanceled_AlreadyCompleted_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var tcs = new TaskCompletionSource<int>();
        tcs.SetResult(42);

        // Act & assert
        Assert.ThrowsAny<InvalidOperationException>(() => tcs.SetCanceled(cancellationToken));
    }
}
