using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class ValueTaskOfTTests
{
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
        var task = new ValueTask<int>(Task.FromResult(42));

        // Assert
        task.IsCompleted.Should().BeTrue();
        task.IsCompletedSuccessfully.Should().BeTrue();
        task.IsFaulted.Should().BeFalse();
        task.IsCanceled.Should().BeFalse();
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
        var task = new ValueTask<int>(Task.FromResult(42));

        // Act
        var result = await task;

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task Await_ConfigureAwait_Test()
    {
        // Arrange
        var task = new ValueTask<int>(Task.FromResult(42));

        // Act
        var result = await task.ConfigureAwait(true);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task Async_Method_Test()
    {
        // Act
        var result = await GetValueAsync();

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task AsTask_Test()
    {
        // Arrange
        var task = new ValueTask<int>(42);

        // Act
        var t = task.AsTask();
        var result = await t;

        // Assert
        t.Should().NotBeNull();
        t.IsCompleted.Should().BeTrue();
        result.Should().Be(42);
    }

    [Fact]
    public void Equals_Test()
    {
        // Arrange
        var a = new ValueTask<int>(42);
        var b = new ValueTask<int>(42);
        var c = new ValueTask<int>(99);

        // Assert
        a.Equals(b).Should().BeTrue();
        a.Equals(c).Should().BeFalse();
        (a == b).Should().BeTrue();
        (a != c).Should().BeTrue();
    }

    private async ValueTask<int> GetValueAsync()
    {
        await Task.Yield();
        return 42;
    }
}
