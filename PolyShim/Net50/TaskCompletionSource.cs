#if FEATURE_TASK
#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Threading.Tasks;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskcompletionsource
#if !POLYFILL_COVERAGE
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
}
#endif
#endif
