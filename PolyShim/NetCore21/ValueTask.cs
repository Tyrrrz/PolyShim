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

    public override string? ToString() => _task?.ToString() ?? string.Empty;

    public static ValueTask CompletedTask => default;

    public static bool operator ==(ValueTask left, ValueTask right) => left.Equals(right);

    public static bool operator !=(ValueTask left, ValueTask right) => !left.Equals(right);
}
#endif
#endif
