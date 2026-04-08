using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

file static class ValueTaskHelpers
{
    public static async ValueTask DoWorkAsync()
    {
        await Task.Yield();
    }

    public static async ValueTask<int> GetValueAsync()
    {
        await DoWorkAsync();
        return 42;
    }
}

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
    public void FromResult_IsCompletedSuccessfully_Test()
    {
        // Act
        var task = new ValueTask<int>(42);

        // Assert
        task.IsCompleted.Should().BeTrue();
        task.IsCompletedSuccessfully.Should().BeTrue();
        task.IsFaulted.Should().BeFalse();
        task.IsCanceled.Should().BeFalse();
    }

    [Fact]
    public async Task FromResult_Result_Test()
    {
        // Act
        var result = await new ValueTask<int>(42);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void FromTask_IsCompletedSuccessfully_Test()
    {
        // Act
        var task = new ValueTask(Task.CompletedTask);
        var taskOfT = new ValueTask<int>(Task.FromResult(42));

        // Assert
        task.IsCompleted.Should().BeTrue();
        task.IsCompletedSuccessfully.Should().BeTrue();
        task.IsFaulted.Should().BeFalse();
        task.IsCanceled.Should().BeFalse();
        taskOfT.IsCompleted.Should().BeTrue();
        taskOfT.IsCompletedSuccessfully.Should().BeTrue();
        taskOfT.IsFaulted.Should().BeFalse();
        taskOfT.IsCanceled.Should().BeFalse();
    }

    [Fact]
    public async Task FromTask_Result_Test()
    {
        // Act
        var result = await new ValueTask<int>(Task.FromResult(42));

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task Await_Test()
    {
        // Arrange
        var task = new ValueTask(Task.CompletedTask);
        var taskOfT = new ValueTask<int>(Task.FromResult(42));

        // Act
        await task;
        var result = await taskOfT;

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task Await_ConfigureAwait_Test()
    {
        // Arrange
        var task = new ValueTask(Task.CompletedTask);
        var taskOfT = new ValueTask<int>(Task.FromResult(42));

        // Act
        await task.ConfigureAwait(true);
        var result = await taskOfT.ConfigureAwait(true);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task Async_Method_Test()
    {
        // Act
        await ValueTaskHelpers.DoWorkAsync();
        var result = await ValueTaskHelpers.GetValueAsync();

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task AsTask_Test()
    {
        // Arrange
        var task = new ValueTask(Task.CompletedTask);
        var taskOfT = new ValueTask<int>(42);

        // Act
        var t = task.AsTask();
        var tOfT = taskOfT.AsTask();
        var result = await tOfT;

        // Assert
        t.Should().NotBeNull();
        t.IsCompleted.Should().BeTrue();
        tOfT.Should().NotBeNull();
        tOfT.IsCompleted.Should().BeTrue();
        result.Should().Be(42);
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
        var e = new ValueTask<int>(42);
        var f = new ValueTask<int>(42);
        var g = new ValueTask<int>(99);

        // Assert
        a.Equals(b).Should().BeTrue();
        c.Equals(d).Should().BeTrue();
        (a == b).Should().BeTrue();
        (c != a).Should().BeTrue();
        e.Equals(f).Should().BeTrue();
        e.Equals(g).Should().BeFalse();
        (e == f).Should().BeTrue();
        (e != g).Should().BeTrue();
    }
}
