using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class TaskTests
{
    [Fact]
    public async Task ConfigureAwait_Options_Test()
    {
        // Arrange
        var task = Task.Delay(10);

        // Act & assert
        await task.ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
    }

    [Fact]
    public async Task ConfigureAwait_Options_None_Test()
    {
        // Arrange
        var task = Task.FromException(new InvalidOperationException());

        // Act & assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await task.ConfigureAwait(ConfigureAwaitOptions.None)
        );
    }

    [Fact]
    public async Task ConfigureAwait_Options_SuppressThrowing_Test()
    {
        // Arrange
        var task = Task.FromException(new InvalidOperationException());

        // Act & assert
        // The faulted task's exception is observed and discarded instead of being propagated.
        await task.ConfigureAwait(
            ConfigureAwaitOptions.ContinueOnCapturedContext | ConfigureAwaitOptions.SuppressThrowing
        );
    }

    [Fact]
    public async Task ConfigureAwait_Options_SuppressThrowing_Cancellation_Test()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var task = Task.FromCanceled(cts.Token);

        // Act & assert
        await task.ConfigureAwait(
            ConfigureAwaitOptions.ContinueOnCapturedContext | ConfigureAwaitOptions.SuppressThrowing
        );
    }

    [Fact]
    public async Task ConfigureAwait_Options_ForceYielding_Test()
    {
        // Arrange
        var task = Task.CompletedTask;

        // Act & assert
        await task.ConfigureAwait(
            ConfigureAwaitOptions.ContinueOnCapturedContext | ConfigureAwaitOptions.ForceYielding
        );
    }

    [Fact]
    public async Task ConfigureAwait_Options_ForceYielding_Exception_Test()
    {
        // Arrange
        var task = Task.FromException(new InvalidOperationException());

        // Act & assert
        // ForceYielding doesn't suppress the exception on its own.
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await task.ConfigureAwait(ConfigureAwaitOptions.ForceYielding)
        );
    }

    [Fact]
    public async Task ConfigureAwait_Options_Invalid_Test()
    {
        // Arrange
        var task = Task.CompletedTask;

        // Act & assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await task.ConfigureAwait((ConfigureAwaitOptions)64)
        );
    }

    [Fact]
    public async Task ConfigureAwait_Result_Options_Test()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var result = await task.ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task ConfigureAwait_Result_Options_None_Test()
    {
        // Arrange
        var task = Task.FromException<int>(new InvalidOperationException());

        // Act & assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await task.ConfigureAwait(ConfigureAwaitOptions.None)
        );
    }

    [Fact]
    public async Task ConfigureAwait_Result_Options_ForceYielding_Test()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var result = await task.ConfigureAwait(
            ConfigureAwaitOptions.ContinueOnCapturedContext | ConfigureAwaitOptions.ForceYielding
        );

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task ConfigureAwait_Result_Options_SuppressThrowing_Test()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act & assert
        // Task<TResult> doesn't support suppressing exceptions, since there's no sensible
        // value to produce as a result in that case.
#pragma warning disable CA2261
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await task.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing)
        );
#pragma warning restore CA2261
    }

    [Fact]
    public async Task ConfigureAwait_Result_Options_Invalid_Test()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act & assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await task.ConfigureAwait((ConfigureAwaitOptions)64)
        );
    }
}
