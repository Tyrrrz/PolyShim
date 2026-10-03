using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net90;

public class TaskCompletionSourceTests
{
    [Fact]
    public async Task SetFromTask_RanToCompletion_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();

        // Act
        tcs.SetFromTask(Task.CompletedTask);
        await tcs.Task;

        // Assert
        tcs.Task.IsCompletedSuccessfully.Should().BeTrue();
    }

    [Fact]
    public async Task SetFromTask_Faulted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var exception = new InvalidOperationException("Test exception");

        // Act
        tcs.SetFromTask(Task.FromException(exception));
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await tcs.Task);

        // Assert
        tcs.Task.IsFaulted.Should().BeTrue();
        ex.Should().Be(exception);
    }

    [Fact]
    public async Task SetFromTask_Canceled_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cancellationToken = new CancellationToken(true);

        // Act
        tcs.SetFromTask(Task.FromCanceled(cancellationToken));
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await tcs.Task
        );

        // Assert
        tcs.Task.IsCanceled.Should().BeTrue();
        ex.CancellationToken.Should().Be(cancellationToken);
    }

    [Fact]
    public void SetFromTask_NotCompleted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var other = new TaskCompletionSource();

        // Act & assert
        Assert.Throws<ArgumentException>(() => tcs.SetFromTask(other.Task));
    }

    [Fact]
    public void SetFromTask_AlreadyCompleted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        tcs.SetResult();

        // Act & assert
        Assert.Throws<InvalidOperationException>(() => tcs.SetFromTask(Task.CompletedTask));
    }

    [Fact]
    public async Task TrySetFromTask_RanToCompletion_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();

        // Act
        var result = tcs.TrySetFromTask(Task.CompletedTask);
        await tcs.Task;

        // Assert
        result.Should().BeTrue();
        tcs.Task.IsCompletedSuccessfully.Should().BeTrue();
    }

    [Fact]
    public async Task TrySetFromTask_Faulted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var exception = new InvalidOperationException("Test exception");

        // Act
        var result = tcs.TrySetFromTask(Task.FromException(exception));
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await tcs.Task);

        // Assert
        result.Should().BeTrue();
        tcs.Task.IsFaulted.Should().BeTrue();
        ex.Should().Be(exception);
    }

    [Fact]
    public async Task TrySetFromTask_Canceled_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var cancellationToken = new CancellationToken(true);

        // Act
        var result = tcs.TrySetFromTask(Task.FromCanceled(cancellationToken));
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await tcs.Task
        );

        // Assert
        result.Should().BeTrue();
        tcs.Task.IsCanceled.Should().BeTrue();
        ex.CancellationToken.Should().Be(cancellationToken);
    }

    [Fact]
    public void TrySetFromTask_AlreadyCompleted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        tcs.SetResult();

        // Act
        var result = tcs.TrySetFromTask(Task.CompletedTask);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void TrySetFromTask_NotCompleted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        var other = new TaskCompletionSource();

        // Act & assert
        Assert.Throws<ArgumentException>(() => tcs.TrySetFromTask(other.Task));
    }

    [Fact]
    public async Task SetFromTask_Result_RanToCompletion_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();

        // Act
        tcs.SetFromTask(Task.FromResult(42));
        var result = await tcs.Task;

        // Assert
        result.Should().Be(42);
        tcs.Task.IsCompletedSuccessfully.Should().BeTrue();
    }

    [Fact]
    public async Task SetFromTask_Result_Faulted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        var exception = new InvalidOperationException("Test exception");

        // Act
        tcs.SetFromTask(Task.FromException<int>(exception));
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await tcs.Task);

        // Assert
        tcs.Task.IsFaulted.Should().BeTrue();
        ex.Should().Be(exception);
    }

    [Fact]
    public async Task SetFromTask_Result_Canceled_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        var cancellationToken = new CancellationToken(true);

        // Act
        tcs.SetFromTask(Task.FromCanceled<int>(cancellationToken));
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await tcs.Task
        );

        // Assert
        tcs.Task.IsCanceled.Should().BeTrue();
        ex.CancellationToken.Should().Be(cancellationToken);
    }

    [Fact]
    public void SetFromTask_Result_NotCompleted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        var other = new TaskCompletionSource<int>();

        // Act & assert
        Assert.Throws<ArgumentException>(() => tcs.SetFromTask(other.Task));
    }

    [Fact]
    public void SetFromTask_Result_AlreadyCompleted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        tcs.SetResult(42);

        // Act & assert
        Assert.Throws<InvalidOperationException>(() => tcs.SetFromTask(Task.FromResult(42)));
    }

    [Fact]
    public async Task TrySetFromTask_Result_RanToCompletion_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();

        // Act
        var result = tcs.TrySetFromTask(Task.FromResult(42));
        var taskResult = await tcs.Task;

        // Assert
        result.Should().BeTrue();
        taskResult.Should().Be(42);
    }

    [Fact]
    public async Task TrySetFromTask_Result_Faulted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        var exception = new InvalidOperationException("Test exception");

        // Act
        var result = tcs.TrySetFromTask(Task.FromException<int>(exception));
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await tcs.Task);

        // Assert
        result.Should().BeTrue();
        tcs.Task.IsFaulted.Should().BeTrue();
        ex.Should().Be(exception);
    }

    [Fact]
    public async Task TrySetFromTask_Result_Canceled_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        var cancellationToken = new CancellationToken(true);

        // Act
        var result = tcs.TrySetFromTask(Task.FromCanceled<int>(cancellationToken));
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await tcs.Task
        );

        // Assert
        result.Should().BeTrue();
        tcs.Task.IsCanceled.Should().BeTrue();
        ex.CancellationToken.Should().Be(cancellationToken);
    }

    [Fact]
    public void TrySetFromTask_Result_AlreadyCompleted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        tcs.SetResult(42);

        // Act
        var result = tcs.TrySetFromTask(Task.FromResult(42));

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void TrySetFromTask_Result_NotCompleted_Test()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        var other = new TaskCompletionSource<int>();

        // Act & assert
        Assert.Throws<ArgumentException>(() => tcs.TrySetFromTask(other.Task));
    }
}
