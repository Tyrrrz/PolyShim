using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

file static class ValueTaskHelpers
{
    public static async ValueTask DoWorkAsync() => await Task.Yield();

    public static async ValueTask<int> GetValueAsync()
    {
        await DoWorkAsync();
        return 42;
    }
}

public class ValueTaskTests
{
    [Fact]
    public void Default_Test()
    {
        // Act
        var task = default(ValueTask);

        // Assert
        task.IsCompleted.Should().BeTrue();
        task.IsCompletedSuccessfully.Should().BeTrue();
        task.IsFaulted.Should().BeFalse();
        task.IsCanceled.Should().BeFalse();
    }

    [Fact]
    public async Task FromResult_Test()
    {
        // Arrange
        var task = new ValueTask<int>(42);

        // Act & assert
        task.IsCompleted.Should().BeTrue();
        task.IsCompletedSuccessfully.Should().BeTrue();
        task.IsFaulted.Should().BeFalse();
        task.IsCanceled.Should().BeFalse();
        (await task).Should().Be(42);
        (await task.ConfigureAwait(true)).Should().Be(42);
    }

    [Fact]
    public async Task FromTask_Test()
    {
        // Arrange
        var task = new ValueTask(Task.CompletedTask);
        var taskOfT = new ValueTask<int>(Task.FromResult(42));

        // Act & assert
        task.IsCompleted.Should().BeTrue();
        task.IsCompletedSuccessfully.Should().BeTrue();
        task.IsFaulted.Should().BeFalse();
        task.IsCanceled.Should().BeFalse();
        await task;
        await task.ConfigureAwait(true);

        taskOfT.IsCompleted.Should().BeTrue();
        taskOfT.IsCompletedSuccessfully.Should().BeTrue();
        taskOfT.IsFaulted.Should().BeFalse();
        taskOfT.IsCanceled.Should().BeFalse();
        (await taskOfT).Should().Be(42);
        (await taskOfT.ConfigureAwait(true)).Should().Be(42);
    }

    [Fact]
    public async Task AsyncMethodBuilder_Test()
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
        var classicTask = task.AsTask();
        var classicTaskOfT = taskOfT.AsTask();
        var result = await classicTaskOfT;

        // Assert
        classicTask.Should().NotBeNull();
        classicTask.IsCompleted.Should().BeTrue();
        classicTaskOfT.Should().NotBeNull();
        classicTaskOfT.IsCompleted.Should().BeTrue();
        result.Should().Be(42);
    }

    [Fact]
    public void Equals_Test()
    {
        // Arrange
        var a = default(ValueTask);
        var b = default(ValueTask);
        var underlyingTask = Task.CompletedTask;
        var c = new ValueTask(underlyingTask);
        var d = new ValueTask(underlyingTask);
        var defaultValueTaskOfT1 = default(ValueTask<int>);
        var defaultValueTaskOfT2 = default(ValueTask<int>);
        var sharedTaskOfT = Task.FromResult(42);
        var e = new ValueTask<int>(sharedTaskOfT);
        var f = new ValueTask<int>(sharedTaskOfT);
        var g = new ValueTask<int>(Task.FromResult(99));
        var resultBackedA = new ValueTask<int>(42);
        var resultBackedB = new ValueTask<int>(42);
        var resultBackedC = new ValueTask<int>(99);

        // Assert
        a.Equals(b).Should().BeTrue();
        c.Equals(d).Should().BeTrue();
        (a == b).Should().BeTrue();
        (c != a).Should().BeTrue();

        defaultValueTaskOfT1.Equals(defaultValueTaskOfT2).Should().BeTrue();
        (defaultValueTaskOfT1 == defaultValueTaskOfT2).Should().BeTrue();

        e.Equals(f).Should().BeTrue();
        e.Equals(g).Should().BeFalse();
        (e == f).Should().BeTrue();
        (e != g).Should().BeTrue();

        resultBackedA.Equals(resultBackedB).Should().BeTrue();
        resultBackedA.Equals(resultBackedC).Should().BeFalse();
        (resultBackedA == resultBackedB).Should().BeTrue();
        (resultBackedA != resultBackedC).Should().BeTrue();
    }
}
