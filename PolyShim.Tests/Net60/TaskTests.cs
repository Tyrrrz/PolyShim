using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class TaskTests
{
    [Fact(Timeout = 5000)]
    public async Task WaitAsync_Token_Test()
    {
        // Arrange
        var task = Task.Delay(TimeSpan.FromSeconds(0.1));

        // Act & assert
        await task.WaitAsync(CancellationToken.None);
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_Token_Cancellation_Test()
    {
        // Arrange
        var task = Task.Delay(Timeout.InfiniteTimeSpan);
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(0.1));

        // Act & assert
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await task.WaitAsync(cts.Token)
        );

        ex.CancellationToken.Should().Be(cts.Token);
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_Timeout_Test()
    {
        // Arrange
        var task = Task.Delay(TimeSpan.FromSeconds(0.1));

        // Act & assert
        await task.WaitAsync(Timeout.InfiniteTimeSpan);
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_Timeout_Cancellation_Test()
    {
        // Arrange
        var task = Task.Delay(Timeout.InfiniteTimeSpan);

        // Act & assert
        await Assert.ThrowsAnyAsync<TimeoutException>(async () =>
            await task.WaitAsync(TimeSpan.FromSeconds(0.1))
        );
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_TokenAndTimeout_Test()
    {
        // Arrange
        var task = Task.Delay(TimeSpan.FromSeconds(0.1));

        // Act & assert
        await task.WaitAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_TokenAndTimeout_CancellationByToken_Test()
    {
        // Arrange
        var task = Task.Delay(Timeout.InfiniteTimeSpan);
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(0.1));

        // Act & assert
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await task.WaitAsync(Timeout.InfiniteTimeSpan, cts.Token)
        );

        ex.CancellationToken.Should().Be(cts.Token);
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_TokenAndTimeout_CancellationByTimeout_Test()
    {
        // Arrange
        var task = Task.Delay(Timeout.InfiniteTimeSpan);

        // Act & assert
        await Assert.ThrowsAnyAsync<TimeoutException>(async () =>
            await task.WaitAsync(TimeSpan.FromSeconds(0.1), CancellationToken.None)
        );
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_Result_Token_Test()
    {
        // Arrange
        var task = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            return 42;
        });

        // Act
        var result = await task.WaitAsync(CancellationToken.None);

        // Assert
        result.Should().Be(42);
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_Result_Token_Cancellation_Test()
    {
        // Arrange
        var task = Task.Run(async () =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan);
            return 42;
        });

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(0.1));

        // Act & assert
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await task.WaitAsync(cts.Token)
        );

        ex.CancellationToken.Should().Be(cts.Token);
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_Result_Timeout_Test()
    {
        // Arrange
        var task = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            return 42;
        });

        // Act
        var result = await task.WaitAsync(Timeout.InfiniteTimeSpan);

        // Assert
        result.Should().Be(42);
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_Result_Timeout_Cancellation_Test()
    {
        // Arrange
        var task = Task.Run(async () =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan);
            return 42;
        });

        // Act & assert
        await Assert.ThrowsAnyAsync<TimeoutException>(async () =>
            await task.WaitAsync(TimeSpan.FromSeconds(0.1))
        );
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_Result_TokenAndTimeout_Test()
    {
        // Arrange
        var task = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            return 42;
        });

        // Act
        var result = await task.WaitAsync(Timeout.InfiniteTimeSpan, CancellationToken.None);

        // Assert
        result.Should().Be(42);
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_Result_TokenAndTimeout_CancellationByToken_Test()
    {
        // Arrange
        var task = Task.Run(async () =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan);
            return 42;
        });

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(0.1));

        // Act & assert
        var ex = await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await task.WaitAsync(Timeout.InfiniteTimeSpan, cts.Token)
        );

        ex.CancellationToken.Should().Be(cts.Token);
    }

    [Fact(Timeout = 5000)]
    public async Task WaitAsync_Result_TokenAndTimeout_CancellationByTimeout_Test()
    {
        // Arrange
        var task = Task.Run(async () =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan);
            return 42;
        });

        // Act & assert
        await Assert.ThrowsAnyAsync<TimeoutException>(async () =>
            await task.WaitAsync(TimeSpan.FromSeconds(0.1), CancellationToken.None)
        );
    }
}