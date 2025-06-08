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
        tcs.TrySetCanceled(cancellationToken);
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await tcs.Task
        );

        // Assert
        tcs.Task.IsCanceled.Should().BeTrue();
        ex.CancellationToken.Should().Be(cancellationToken);
        ex.CancellationToken.IsCancellationRequested.Should().BeTrue();
    }
}
