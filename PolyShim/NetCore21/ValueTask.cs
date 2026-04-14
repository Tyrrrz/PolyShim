#if FEATURE_TASK
#if !FEATURE_VALUETASK
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace System.Threading.Tasks;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask
[AsyncMethodBuilder(typeof(AsyncValueTaskMethodBuilder))]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTask(Task task) : IEquatable<ValueTask>
{
    private readonly Task? _task = task ?? throw new ArgumentNullException(nameof(task));

    public ValueTask(IValueTaskSource source, short token)
        : this(WrapVoidSource(source ?? throw new ArgumentNullException(nameof(source)), token)) { }

    private static void CompleteVoidFromSource(
        TaskCompletionSource<bool> tcs,
        IValueTaskSource source,
        short token
    )
    {
        switch (source.GetStatus(token))
        {
            case ValueTaskSourceStatus.Succeeded:
                source.GetResult(token);
                tcs.TrySetResult(false);
                break;
            case ValueTaskSourceStatus.Faulted:
                try
                {
                    source.GetResult(token);
                    tcs.TrySetException(
                        new InvalidOperationException(
                            "Source reported faulted status but GetResult did not throw."
                        )
                    );
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }

                break;
            default:
                tcs.TrySetCanceled();
                break;
        }
    }

    private static Task WrapVoidSource(IValueTaskSource source, short token)
    {
        var tcs = new TaskCompletionSource<bool>();

        if (source.GetStatus(token) != ValueTaskSourceStatus.Pending)
        {
            CompleteVoidFromSource(tcs, source, token);
            return tcs.Task;
        }

        source.OnCompleted(
            _ => CompleteVoidFromSource(tcs, source, token),
            null,
            token,
            ValueTaskSourceOnCompletedFlags.None
        );

        return tcs.Task;
    }

    public bool IsCompleted => _task is null || _task.IsCompleted;

    public bool IsCompletedSuccessfully =>
        _task is null || _task.Status == TaskStatus.RanToCompletion;

    public bool IsFaulted => _task is not null && _task.IsFaulted;

    public bool IsCanceled => _task is not null && _task.IsCanceled;

    public Task AsTask() => _task ?? Task.CompletedTask;

    public ValueTaskAwaiter GetAwaiter() => new(this);

    public ConfiguredValueTaskAwaitable ConfigureAwait(bool continueOnCapturedContext) =>
        new(this, continueOnCapturedContext);

    public bool Equals(ValueTask other) => _task == other._task;

    public override bool Equals(object? obj) => obj is ValueTask other && Equals(other);

    public override int GetHashCode() => _task?.GetHashCode() ?? 0;

    public override string? ToString() => _task?.ToString();

    public static ValueTask CompletedTask => default;

    public static bool operator ==(ValueTask left, ValueTask right) => left.Equals(right);

    public static bool operator !=(ValueTask left, ValueTask right) => !left.Equals(right);
}

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1
[AsyncMethodBuilder(typeof(AsyncValueTaskMethodBuilder<>))]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTask<TResult>(Task<TResult> task) : IEquatable<ValueTask<TResult>>
{
    private readonly Task<TResult>? _task = task ?? throw new ArgumentNullException(nameof(task));

    public ValueTask(TResult result)
        : this(Task.FromResult(result)) { }

    public ValueTask(IValueTaskSource<TResult> source, short token)
        : this(WrapSource(source ?? throw new ArgumentNullException(nameof(source)), token)) { }

    private static void CompleteFromSource(
        TaskCompletionSource<TResult> tcs,
        IValueTaskSource<TResult> source,
        short token
    )
    {
        switch (source.GetStatus(token))
        {
            case ValueTaskSourceStatus.Succeeded:
                tcs.TrySetResult(source.GetResult(token));
                break;
            case ValueTaskSourceStatus.Faulted:
                try
                {
                    source.GetResult(token);
                    tcs.TrySetException(
                        new InvalidOperationException(
                            "Source reported faulted status but GetResult did not throw."
                        )
                    );
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }

                break;
            default:
                tcs.TrySetCanceled();
                break;
        }
    }

    private static Task<TResult> WrapSource(IValueTaskSource<TResult> source, short token)
    {
        if (source.GetStatus(token) == ValueTaskSourceStatus.Succeeded)
            return Task.FromResult(source.GetResult(token));

        var tcs = new TaskCompletionSource<TResult>();

        if (source.GetStatus(token) != ValueTaskSourceStatus.Pending)
        {
            CompleteFromSource(tcs, source, token);
            return tcs.Task;
        }

        source.OnCompleted(
            _ => CompleteFromSource(tcs, source, token),
            null,
            token,
            ValueTaskSourceOnCompletedFlags.None
        );
        return tcs.Task;
    }

    public bool IsCompleted => _task is null || _task.IsCompleted;

    public bool IsCompletedSuccessfully =>
        _task is null || _task.Status == TaskStatus.RanToCompletion;

    public bool IsFaulted => _task is not null && _task.IsFaulted;

    public bool IsCanceled => _task is not null && _task.IsCanceled;

    public TResult Result => _task is null ? default! : _task.GetAwaiter().GetResult();

    public Task<TResult> AsTask() => _task ?? Task.FromResult(default(TResult)!);

    public ValueTaskAwaiter<TResult> GetAwaiter() => new(this);

    public ConfiguredValueTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext) =>
        new(this, continueOnCapturedContext);

    public bool Equals(ValueTask<TResult> other) => _task == other._task;

    public override bool Equals(object? obj) => obj is ValueTask<TResult> other && Equals(other);

    public override int GetHashCode() => _task?.GetHashCode() ?? 0;

    public override string? ToString() => _task?.ToString();

    public static bool operator ==(ValueTask<TResult> left, ValueTask<TResult> right) =>
        left.Equals(right);

    public static bool operator !=(ValueTask<TResult> left, ValueTask<TResult> right) =>
        !left.Equals(right);
}
#endif
#endif
