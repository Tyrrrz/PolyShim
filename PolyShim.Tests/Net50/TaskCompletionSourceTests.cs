using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

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
        var act = async () => await tcs.Task;
        var ex = (await act.Should().ThrowAsync<InvalidOperationException>()).Which;

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
        var act = async () => await tcs.Task;
        await act.Should().ThrowAsync<OperationCanceledException>();

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
        var act = async () => await tcs.Task;
        await act.Should().ThrowAsync<OperationCanceledException>();

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
        var act = () => tcs.SetCanceled(new CancellationToken(true));
        act.Should().Throw<InvalidOperationException>();
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
        var act = async () => await tcs.Task;
        var ex = (await act.Should().ThrowAsync<InvalidOperationException>()).Which;

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
        var act = async () => await tcs.Task;
        await act.Should().ThrowAsync<OperationCanceledException>();

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
        var act = async () => await tcs.Task;
        await act.Should().ThrowAsync<OperationCanceledException>();

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

    [Fact]
    public async Task SetCanceled_Result_Test()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);
        var tcs = new TaskCompletionSource<int>();

        // Act
        tcs.SetCanceled(cancellationToken);
        var act = async () => await tcs.Task;
        var ex = (await act.Should().ThrowAsync<OperationCanceledException>()).Which;

        // Assert
        tcs.Task.IsCanceled.Should().BeTrue();
        ex.CancellationToken.Should().Be(cancellationToken);
        ex.CancellationToken.IsCancellationRequested.Should().BeTrue();
    }

    [Fact]
    public void SetCanceled_Result_AlreadyCompleted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        tcs.SetResult(42);

        // Act & assert
        var act = () => tcs.SetCanceled(new CancellationToken(true));
        act.Should().Throw<InvalidOperationException>();
    }
}
