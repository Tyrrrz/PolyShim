using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class TaskTests
{
    [Fact]
    public async Task WaitAsync_NoResult_Negative_ByTokenToken_Positive_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var task = Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);

        // Act & assert
        await task.WaitAsync(cts.Token);
    }

    [Fact]
    public async Task WaitAsync_NoResult_Negative_ByTokenToken_Negative_ByToken_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(0.01));
        var task = Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);

        // Act
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await task.WaitAsync(cts.Token)
        );

        // Assert
        ex.CancellationToken.Should().Be(cts.Token);
    }

    [Fact]
    public async Task WaitAsync_NoResult_TimeSpan_Positive_Test()
    {
        // Arrange
        var task = Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);

        // Act & assert
        await task.WaitAsync(TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task WaitAsync_NoResult_TimeSpan_Negative_ByTimeout_Test()
    {
        // Arrange
        var task = Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);

        // Act & assert
        await Assert.ThrowsAnyAsync<TimeoutException>(async () =>
            await task.WaitAsync(TimeSpan.FromSeconds(0.01))
        );
    }

    [Fact]
    public async Task WaitAsync_NoResult_TimeSpanAndCancellationToken_Positive_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var task = Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);

        // Act & assert
        await task.WaitAsync(TimeSpan.FromSeconds(1), cts.Token);
    }

    [Fact]
    public async Task WaitAsync_NoResult_TimeSpanAndCancellationToken_Negative_ByToken_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(0.01));
        var task = Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);

        // Act
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await task.WaitAsync(TimeSpan.FromSeconds(1), cts.Token)
        );

        // Assert
        ex.CancellationToken.Should().Be(cts.Token);
    }

    [Fact]
    public async Task WaitAsync_NoResult_TimeSpanAndCancellationToken_Negative_ByTimeout_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var task = Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);

        // Act & assert
        await Assert.ThrowsAnyAsync<TimeoutException>(async () =>
            await task.WaitAsync(TimeSpan.FromSeconds(0.01), cts.Token)
        );
    }

    [Fact]
    public async Task WaitAsync_WithResult_Negative_ByTokenToken_Positive_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var task = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);
            return 42;
        }, CancellationToken.None);

        // Act
        var result = await task.WaitAsync(cts.Token);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task WaitAsync_WithResult_Negative_ByTokenToken_Negative_ByToken_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(0.01));
        var task = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);
            return 42;
        }, CancellationToken.None);

        // Act
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await task.WaitAsync(cts.Token)
        );

        // Assert
        ex.CancellationToken.Should().Be(cts.Token);
    }

    [Fact]
    public async Task WaitAsync_WithResult_TimeSpan_Positive_Test()
    {
        // Arrange
        var task = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);
            return 42;
        }, CancellationToken.None);

        // Act
        var result = await task.WaitAsync(TimeSpan.FromSeconds(1));

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task WaitAsync_WithResult_TimeSpan_Negative_ByTimeout_Test()
    {
        // Arrange
        var task = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);
            return 42;
        }, CancellationToken.None);

        // Act & assert
        await Assert.ThrowsAnyAsync<TimeoutException>(async () =>
            await task.WaitAsync(TimeSpan.FromSeconds(0.01))
        );
    }

    [Fact]
    public async Task WaitAsync_WithResult_TimeSpanAndCancellationToken_Positive_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var task = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);
            return 42;
        }, CancellationToken.None);

        // Act
        var result = await task.WaitAsync(TimeSpan.FromSeconds(1), cts.Token);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task WaitAsync_WithResult_TimeSpanAndCancellationToken_Negative_ByToken_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(0.01));
        var task = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);
            return 42;
        }, CancellationToken.None);

        // Act
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await task.WaitAsync(TimeSpan.FromSeconds(1), cts.Token)
        );

        // Assert
        ex.CancellationToken.Should().Be(cts.Token);
    }

    [Fact]
    public async Task WaitAsync_WithResult_TimeSpanAndCancellationToken_Negative_ByTimeout_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var task = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1), CancellationToken.None);
            return 42;
        }, CancellationToken.None);

        // Act & assert
        await Assert.ThrowsAnyAsync<TimeoutException>(async () =>
            await task.WaitAsync(TimeSpan.FromSeconds(0.01), cts.Token)
        );
    }
}