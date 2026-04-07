#if !FEATURE_VALUETASK && FEATURE_TASK
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Threading.Tasks
{
    // https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask
    [AsyncMethodBuilder(typeof(AsyncValueTaskMethodBuilder))]
#if !POLYFILL_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    internal readonly struct ValueTask : IEquatable<ValueTask>
    {
        private readonly Task? _task;

        public ValueTask(Task task)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
        }

        public static ValueTask CompletedTask => default;

        public bool IsCompleted => _task == null || _task.IsCompleted;

        public bool IsCompletedSuccessfully =>
            _task == null || _task.Status == TaskStatus.RanToCompletion;

        public bool IsFaulted => _task != null && _task.IsFaulted;

        public bool IsCanceled => _task != null && _task.IsCanceled;

        public Task AsTask() => _task ?? Task.CompletedTask;

        public ValueTaskAwaiter GetAwaiter() => new ValueTaskAwaiter(this);

        public ConfiguredValueTaskAwaitable ConfigureAwait(bool continueOnCapturedContext) =>
            new ConfiguredValueTaskAwaitable(this, continueOnCapturedContext);

        public bool Equals(ValueTask other) => _task == other._task;

        public override bool Equals(object? obj) => obj is ValueTask other && Equals(other);

        public override int GetHashCode() => _task?.GetHashCode() ?? 0;

        public override string? ToString() => _task?.ToString() ?? string.Empty;

        public static bool operator ==(ValueTask left, ValueTask right) => left.Equals(right);

        public static bool operator !=(ValueTask left, ValueTask right) => !left.Equals(right);
    }
}

namespace System.Runtime.CompilerServices
{
    // https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.valuetaskawaiter
#if !POLYFILL_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    internal readonly struct ValueTaskAwaiter : INotifyCompletion, ICriticalNotifyCompletion
    {
        private readonly ValueTask _value;

        public ValueTaskAwaiter(ValueTask value) => _value = value;

        public bool IsCompleted => _value.IsCompleted;

        public void GetResult() => _value.AsTask().GetAwaiter().GetResult();

        public void OnCompleted(Action continuation) =>
            _value.AsTask().GetAwaiter().OnCompleted(continuation);

        public void UnsafeOnCompleted(Action continuation) =>
            _value.AsTask().GetAwaiter().UnsafeOnCompleted(continuation);
    }

    // https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.configuredvaluetaskawaitable
#if !POLYFILL_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    internal readonly struct ConfiguredValueTaskAwaitable
    {
        private readonly ValueTask _value;
        private readonly bool _continueOnCapturedContext;

        public ConfiguredValueTaskAwaitable(ValueTask value, bool continueOnCapturedContext)
        {
            _value = value;
            _continueOnCapturedContext = continueOnCapturedContext;
        }

        public Awaiter GetAwaiter() => new Awaiter(_value, _continueOnCapturedContext);

#if !POLYFILL_COVERAGE
        [ExcludeFromCodeCoverage]
#endif
        public readonly struct Awaiter : INotifyCompletion, ICriticalNotifyCompletion
        {
            private readonly ValueTask _value;
            private readonly bool _continueOnCapturedContext;

            public Awaiter(ValueTask value, bool continueOnCapturedContext)
            {
                _value = value;
                _continueOnCapturedContext = continueOnCapturedContext;
            }

            public bool IsCompleted => _value.IsCompleted;

            public void GetResult() => _value.AsTask().GetAwaiter().GetResult();

            public void OnCompleted(Action continuation) =>
                _value
                    .AsTask()
                    .ConfigureAwait(_continueOnCapturedContext)
                    .GetAwaiter()
                    .OnCompleted(continuation);

            public void UnsafeOnCompleted(Action continuation) =>
                _value
                    .AsTask()
                    .ConfigureAwait(_continueOnCapturedContext)
                    .GetAwaiter()
                    .UnsafeOnCompleted(continuation);
        }
    }

    // https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.asyncvaluetaskmethodbuilder
#if !POLYFILL_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    internal struct AsyncValueTaskMethodBuilder
    {
        private AsyncTaskMethodBuilder _taskBuilder;

        public static AsyncValueTaskMethodBuilder Create() =>
            new AsyncValueTaskMethodBuilder { _taskBuilder = AsyncTaskMethodBuilder.Create() };

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine => _taskBuilder.Start(ref stateMachine);

        public void SetStateMachine(IAsyncStateMachine stateMachine) =>
            _taskBuilder.SetStateMachine(stateMachine);

        public void SetResult() => _taskBuilder.SetResult();

        public void SetException(Exception exception) => _taskBuilder.SetException(exception);

        public ValueTask Task => new ValueTask(_taskBuilder.Task);

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter,
            ref TStateMachine stateMachine
        )
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine =>
            _taskBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter,
            ref TStateMachine stateMachine
        )
            where TAwaiter : struct, ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine =>
            _taskBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
    }
}
#endif
