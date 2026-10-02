#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// Task is not available on all target frameworks within this TFM range without a NuGet package reference
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskcompletionsource
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class TaskCompletionSource(object? state, TaskCreationOptions creationOptions)
{
    private readonly TaskCompletionSource<object?> _source = new(state, creationOptions);

    public TaskCompletionSource(object? state)
        : this(state, TaskCreationOptions.None) { }

    public TaskCompletionSource(TaskCreationOptions creationOptions)
        : this(null, creationOptions) { }

    public TaskCompletionSource()
        : this(null, TaskCreationOptions.None) { }

    public Task Task => _source.Task;

    public void SetResult() => _source.SetResult(null);

    public void SetException(Exception exception) => _source.SetException(exception);

    public void SetCanceled() => _source.SetCanceled();

    public void SetCanceled(CancellationToken cancellationToken) =>
        _source.SetCanceled(cancellationToken);

    public bool TrySetResult() => _source.TrySetResult(null);

    public bool TrySetException(Exception exception) => _source.TrySetException(exception);

    public bool TrySetCanceled() => _source.TrySetCanceled();

    public bool TrySetCanceled(CancellationToken cancellationToken) =>
        _source.TrySetCanceled(cancellationToken);

    // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskcompletionsource.setfromtask
    public void SetFromTask(Task completedTask)
    {
        if (!TrySetFromTask(completedTask))
        {
            throw new InvalidOperationException(
                "The task is already completed, canceled, or failed."
            );
        }
    }

    // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskcompletionsource.trysetfromtask
    public bool TrySetFromTask(Task completedTask)
    {
        ArgumentNullException.ThrowIfNull(completedTask);

        if (!completedTask.IsCompleted)
        {
            throw new ArgumentException(
                "The task must already be completed.",
                nameof(completedTask)
            );
        }

        return completedTask.Status switch
        {
            TaskStatus.RanToCompletion => _source.TrySetResult(null),
            TaskStatus.Faulted => _source.TrySetException(completedTask.Exception!.InnerExceptions),
            _ => _source.TrySetCanceled(GetCancellationToken(completedTask)),
        };
    }

    // Task does not expose the token that canceled it, so it needs to be recovered by observing
    // the task directly.
    private static CancellationToken GetCancellationToken(Task canceledTask)
    {
        try
        {
            canceledTask.GetAwaiter().GetResult();
        }
        catch (OperationCanceledException ex)
        {
            return ex.CancellationToken;
        }
        catch
        {
            // Ignore other exceptions; fall back to an empty token below.
        }

        return default;
    }
}
#endif
#endif
