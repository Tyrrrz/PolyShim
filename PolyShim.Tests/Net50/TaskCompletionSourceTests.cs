using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

// Tests for the polyfills for the non-generic TaskCompletionSource.
// The whole type is polyfilled.
public class TaskCompletionSourceTests
{
    [Fact]
    public async Task SetResult_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();

        // Act
        tcs.SetResult();
        await tcs.Task;

        // Assert
        tcs.Task.IsCompletedSuccessfully.Should().BeTrue();
    }

    [Fact]
    public async Task SetException_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var exception = new InvalidOperationException("Test exception");

        // Act
        tcs.SetException(exception);
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await tcs.Task);

        // Assert
        tcs.Task.IsFaulted.Should().BeTrue();
        ex.Should().Be(exception);
    }

    [Fact]
    public async Task SetCanceled_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();

        // Act
        tcs.SetCanceled();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await tcs.Task);

        // Assert
        tcs.Task.IsCanceled.Should().BeTrue();
    }

    [Fact]
    public async Task SetCanceled_WithCancellationToken_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var tcs = new TaskCompletionSource();

        // Act
        tcs.SetCanceled(cancellationToken);
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await tcs.Task);

        // Assert
        tcs.Task.IsCanceled.Should().BeTrue();
    }

    [Fact]
    public void SetCanceled_AlreadyCompleted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        tcs.SetResult();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            tcs.SetCanceled(new CancellationToken(true))
        );
    }

    [Fact]
    public void TrySetResult_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();

        // Act
        var result = tcs.TrySetResult();

        // Assert
        result.Should().BeTrue();
        tcs.Task.IsCompletedSuccessfully.Should().BeTrue();
    }

    [Fact]
    public async Task TrySetException_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var exception = new InvalidOperationException("Test exception");

        // Act
        var result = tcs.TrySetException(exception);
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await tcs.Task);

        // Assert
        result.Should().BeTrue();
        tcs.Task.IsFaulted.Should().BeTrue();
        ex.Should().Be(exception);
    }

    [Fact]
    public async Task TrySetCanceled_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();

        // Act
        var result = tcs.TrySetCanceled();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await tcs.Task);

        // Assert
        result.Should().BeTrue();
        tcs.Task.IsCanceled.Should().BeTrue();
    }

    [Fact]
    public async Task TrySetCanceled_WithCancellationToken_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var tcs = new TaskCompletionSource();

        // Act
        var result = tcs.TrySetCanceled(cancellationToken);
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await tcs.Task);

        // Assert
        result.Should().BeTrue();
        tcs.Task.IsCanceled.Should().BeTrue();
    }

    [Fact]
    public void TrySetCanceled_AlreadyCompleted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        tcs.SetResult();

        // Act
        var result = tcs.TrySetCanceled(new CancellationToken(true));

        // Assert
        result.Should().BeFalse();
    }
}
