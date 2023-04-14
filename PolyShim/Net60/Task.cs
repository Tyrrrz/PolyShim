#if FEATURE_TASK
#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

[ExcludeFromCodeCoverage]
internal static class _D314682FE30A40479B425C42FF0A3E9C
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.waitasync#system-threading-tasks-task-waitasync(system-timespan-system-threading-cancellationtoken)
    public static async Task WaitAsync(this Task task, TimeSpan timeout, CancellationToken cancellationToken)
    {
        var cancellationTask = Task.Delay(timeout, cancellationToken);
        var finishedTask = await Task.WhenAny(task, cancellationTask).ConfigureAwait(false);

        // Finalize and propagate exceptions
        await finishedTask.ConfigureAwait(false);

        if (finishedTask == cancellationTask)
            throw new TimeoutException("The operation has timed out.");
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.waitasync#system-threading-tasks-task-waitasync(system-threading-cancellationtoken)
    public static async Task WaitAsync(this Task task, CancellationToken cancellationToken) =>
        await task.WaitAsync(Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);

    // https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.waitasync#system-threading-tasks-task-waitasync(system-timespan)
    public static async Task WaitAsync(this Task task, TimeSpan timeout) =>
        await task.WaitAsync(timeout, CancellationToken.None).ConfigureAwait(false);

    // https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1.waitasync#system-threading-tasks-task-1-waitasync(system-timespan-system-threading-cancellationtoken)
    public static async Task<T> WaitAsync<T>(this Task<T> task, TimeSpan timeout, CancellationToken cancellationToken)
    {
        var cancellationTask = Task.Delay(timeout, cancellationToken);
        var finishedTask = await Task.WhenAny(task, cancellationTask).ConfigureAwait(false);

        // Finalize and propagate exceptions
        await finishedTask.ConfigureAwait(false);

        if (finishedTask == cancellationTask)
            throw new TimeoutException("The operation has timed out.");

        // If the exception is not thrown, we can safely assume that the main task is completed
        return task.Result;
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1.waitasync#system-threading-tasks-task-1-waitasync(system-threading-cancellationtoken)
    public static async Task<T> WaitAsync<T>(this Task<T> task, CancellationToken cancellationToken) =>
        await task.WaitAsync(Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);

    // https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1.waitasync#system-threading-tasks-task-1-waitasync(system-timespan)
    public static async Task<T> WaitAsync<T>(this Task<T> task, TimeSpan timeout) =>
        await task.WaitAsync(timeout, CancellationToken.None).ConfigureAwait(false);
}
#endif
#endif