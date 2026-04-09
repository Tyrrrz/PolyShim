#if FEATURE_TASK
// Compatibility package that provides FEATURE_VALUETASK doesn't backport this specific type on some target frameworks
#if !FEATURE_VALUETASK || (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK && !NET461_OR_GREATER)
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

    private TaskCompletionSource<TResult> GetOrCreateTcs() => _tcs ??= new();

    public void SetResult(TResult result) => GetOrCreateTcs().TrySetResult(result);

    public void SetException(Exception error)
    {
        if (error is OperationCanceledException oce)
            GetOrCreateTcs().TrySetCanceled(oce.CancellationToken);
        else
            GetOrCreateTcs().TrySetException(error);
    }

    private void ValidateToken(short token)
    {
        if (token != _version)
            throw new InvalidOperationException();
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

        var capturedContext =
            (flags & ValueTaskSourceOnCompletedFlags.UseSchedulingContext) != 0
                ? SynchronizationContext.Current
                : null;

        GetOrCreateTcs()
            .Task.ContinueWith(
                _ =>
                {
                    if (capturedContext is not null)
                    {
                        capturedContext.Post(_ => continuation(state), null);
                    }
                    else
                    {
                        continuation(state);
                    }
                },
                CancellationToken.None,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                RunContinuationsAsynchronously
                    ? TaskContinuationOptions.RunContinuationsAsynchronously
                    : TaskContinuationOptions.ExecuteSynchronously,
#else
                TaskContinuationOptions.ExecuteSynchronously,
#endif
                TaskScheduler.Default
            );
    }
}
#endif
#endif
