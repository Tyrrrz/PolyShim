using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class ValueTaskTests
{
    [Fact]
    public void Default_IsCompletedSuccessfully_Test()
    {
        // Act
        var task = ValueTask.CompletedTask;

        // Assert
        task.IsCompleted.Should().BeTrue();
        task.IsCompletedSuccessfully.Should().BeTrue();
        task.IsFaulted.Should().BeFalse();
        task.IsCanceled.Should().BeFalse();
    }

    [Fact]
    public void FromTask_IsCompletedSuccessfully_Test()
    {
        // Act
        var task = new ValueTask(Task.CompletedTask);

        // Assert
        task.IsCompleted.Should().BeTrue();
        task.IsCompletedSuccessfully.Should().BeTrue();
        task.IsFaulted.Should().BeFalse();
        task.IsCanceled.Should().BeFalse();
    }

    [Fact]
    public async Task Await_Test()
    {
        // Arrange
        var task = new ValueTask(Task.CompletedTask);

        // Act & Assert (no exception)
        await task;
    }

    [Fact]
    public async Task Await_ConfigureAwait_Test()
    {
        // Arrange
        var task = new ValueTask(Task.CompletedTask);

        // Act & Assert (no exception)
        await task.ConfigureAwait(true);
    }

    [Fact]
    public async Task Async_Method_Test()
    {
        // Act & Assert (no exception)
        await DoWorkAsync();
    }

    [Fact]
    public void AsTask_Test()
    {
        // Arrange
        var task = new ValueTask(Task.CompletedTask);

        // Act
        var t = task.AsTask();

        // Assert
        t.Should().NotBeNull();
        t.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public void Equals_Test()
    {
        // Arrange
        var a = ValueTask.CompletedTask;
        var b = ValueTask.CompletedTask;
        var underlyingTask = Task.CompletedTask;
        var c = new ValueTask(underlyingTask);
        var d = new ValueTask(underlyingTask);

        // Assert
        a.Equals(b).Should().BeTrue();
        c.Equals(d).Should().BeTrue();
        (a == b).Should().BeTrue();
        (c != a).Should().BeTrue();
    }

    private async ValueTask DoWorkAsync()
    {
        await Task.Yield();
    }
}
