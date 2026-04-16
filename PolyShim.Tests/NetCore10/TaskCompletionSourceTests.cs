using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class TaskCompletionSourceTests
{
    [Fact]
    public async Task TrySetCanceled_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var tcs = new TaskCompletionSource<int>();

        // Act
        var result = tcs.TrySetCanceled(cancellationToken);
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await tcs.Task
        );

        // Assert
        result.Should().BeTrue();
        tcs.Task.IsCanceled.Should().BeTrue();
        ex.CancellationToken.Should().Be(cancellationToken);
        ex.CancellationToken.IsCancellationRequested.Should().BeTrue();
    }

    [Fact]
    public void TrySetCanceled_AlreadyCompleted_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var tcs = new TaskCompletionSource<int>();
        tcs.SetResult(42);

        // Act
        var result = tcs.TrySetCanceled(cancellationToken);

        // Assert
        result.Should().BeFalse();
    }
}
