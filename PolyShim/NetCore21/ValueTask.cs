#if !FEATURE_VALUETASK
#if FEATURE_TASK
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Threading.Tasks;

// https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask
[AsyncMethodBuilder(typeof(AsyncValueTaskMethodBuilder))]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTask(Task task) : IEquatable<ValueTask>
{
    private readonly Task? _task = task ?? throw new ArgumentNullException(nameof(task));

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
    private readonly Task<TResult> _task = task ?? throw new ArgumentNullException(nameof(task));

    private static Task<TResult> CreateTask(TResult result)
    {
        var tcs = new TaskCompletionSource<TResult>();
        tcs.SetResult(result);
        return tcs.Task;
    }

    public ValueTask(TResult result)
        : this(CreateTask(result)) { }

    public bool IsCompleted => _task.IsCompleted;

    public bool IsCompletedSuccessfully => _task.Status == TaskStatus.RanToCompletion;

    public bool IsFaulted => _task.IsFaulted;

    public bool IsCanceled => _task.IsCanceled;

    public TResult Result => _task.GetAwaiter().GetResult();

    public Task<TResult> AsTask() => _task;

    public ValueTaskAwaiter<TResult> GetAwaiter() => new(this);

    public ConfiguredValueTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext) =>
        new(this, continueOnCapturedContext);

    public bool Equals(ValueTask<TResult> other) => _task == other._task;

    public override bool Equals(object? obj) => obj is ValueTask<TResult> other && Equals(other);

    public override int GetHashCode() => _task.GetHashCode();

    public override string? ToString() => _task.ToString();

    public static bool operator ==(ValueTask<TResult> left, ValueTask<TResult> right) =>
        left.Equals(right);

    public static bool operator !=(ValueTask<TResult> left, ValueTask<TResult> right) =>
        !left.Equals(right);
}
#endif
#endif
