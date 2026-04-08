#if !FEATURE_VALUETASK
#if FEATURE_TASK
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace System.Threading.Tasks.Sources;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.sources.valuetasksourcestatus
internal enum ValueTaskSourceStatus
{
    Pending = 0,
    Succeeded = 1,
    Faulted = 2,
    Canceled = 3,
}

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.sources.valuetasksourceoncompletedflag
[Flags]
internal enum ValueTaskSourceOnCompletedFlags
{
    None = 0,
    UseSchedulingContext = 1,
    FlowExecutionContext = 2,
}

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.sources.ivaluetasksource
internal interface IValueTaskSource
{
    ValueTaskSourceStatus GetStatus(short token);

    void OnCompleted(
        Action<object?> continuation,
        object? state,
        short token,
        ValueTaskSourceOnCompletedFlags flags
    );

    void GetResult(short token);
}

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.sources.ivaluetasksource-1
internal interface IValueTaskSource<out TResult>
{
    ValueTaskSourceStatus GetStatus(short token);

    void OnCompleted(
        Action<object?> continuation,
        object? state,
        short token,
        ValueTaskSourceOnCompletedFlags flags
    );

    TResult GetResult(short token);
}

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.sources.manualresetvaluetasksourcecore-1
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal struct ManualResetValueTaskSourceCore<TResult>
{
    private TaskCompletionSource<TResult>? _tcs;
    private short _version;

    public bool RunContinuationsAsynchronously { get; set; }

    public short Version => _version;

    public void Reset()
    {
        unchecked
        {
            _version++;
        }

        _tcs = null;
    }

    private TaskCompletionSource<TResult> GetOrCreateTcs()
    {
        if (_tcs is null)
            _tcs = new TaskCompletionSource<TResult>();

        return _tcs;
    }

    public void SetResult(TResult result) => GetOrCreateTcs().TrySetResult(result);

    public void SetException(Exception error)
    {
        if (error is OperationCanceledException)
            GetOrCreateTcs().TrySetCanceled();
        else
            GetOrCreateTcs().TrySetException(error);
    }

    public TResult GetResult(short token)
    {
        ValidateToken(token);
        return _tcs is null ? default! : _tcs.Task.GetAwaiter().GetResult();
    }

    public ValueTaskSourceStatus GetStatus(short token)
    {
        ValidateToken(token);

        if (_tcs is null || !_tcs.Task.IsCompleted)
            return ValueTaskSourceStatus.Pending;

        if (_tcs.Task.IsCanceled)
            return ValueTaskSourceStatus.Canceled;

        if (_tcs.Task.IsFaulted)
            return ValueTaskSourceStatus.Faulted;

        return ValueTaskSourceStatus.Succeeded;
    }

    public void OnCompleted(
        Action<object?> continuation,
        object? state,
        short token,
        ValueTaskSourceOnCompletedFlags flags
    )
    {
        ValidateToken(token);
        GetOrCreateTcs()
            .Task.ContinueWith(
                _ => continuation(state),
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default
            );
    }

    private void ValidateToken(short token)
    {
        if (token != _version)
            throw new InvalidOperationException();
    }
}
#endif
#endif
