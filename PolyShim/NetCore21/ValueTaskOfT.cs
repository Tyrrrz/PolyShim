#if !FEATURE_VALUETASK && FEATURE_TASK
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Threading.Tasks
{
    // https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1
    [AsyncMethodBuilder(typeof(AsyncValueTaskMethodBuilder<>))]
#if !POLYFILL_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    internal readonly struct ValueTask<TResult> : IEquatable<ValueTask<TResult>>
    {
        private readonly Task<TResult>? _task;
        private readonly TResult _result;

        public ValueTask(TResult result)
        {
            _task = null;
            _result = result;
        }

        public ValueTask(Task<TResult> task)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
            _result = default!;
        }

        public bool IsCompleted => _task == null || _task.IsCompleted;

        public bool IsCompletedSuccessfully =>
            _task == null || _task.Status == TaskStatus.RanToCompletion;

        public bool IsFaulted => _task != null && _task.IsFaulted;

        public bool IsCanceled => _task != null && _task.IsCanceled;

        public TResult Result => _task == null ? _result : _task.GetAwaiter().GetResult();

        public Task<TResult> AsTask()
        {
            if (_task != null)
                return _task;

            var tcs = new TaskCompletionSource<TResult>();
            tcs.SetResult(_result);
            return tcs.Task;
        }

        public ValueTaskAwaiter<TResult> GetAwaiter() => new ValueTaskAwaiter<TResult>(this);

        public ConfiguredValueTaskAwaitable<TResult> ConfigureAwait(
            bool continueOnCapturedContext
        ) => new ConfiguredValueTaskAwaitable<TResult>(this, continueOnCapturedContext);

        public bool Equals(ValueTask<TResult> other)
        {
            if (_task == null && other._task == null)
                return EqualityComparer<TResult>.Default.Equals(_result, other._result);

            return _task == other._task;
        }

        public override bool Equals(object? obj) =>
            obj is ValueTask<TResult> other && Equals(other);

        public override int GetHashCode() =>
            _task != null ? _task.GetHashCode() : _result?.GetHashCode() ?? 0;

        public override string? ToString() =>
            _task == null ? _result?.ToString() : _task.ToString();

        public static bool operator ==(ValueTask<TResult> left, ValueTask<TResult> right) =>
            left.Equals(right);

        public static bool operator !=(ValueTask<TResult> left, ValueTask<TResult> right) =>
            !left.Equals(right);
    }
}

namespace System.Runtime.CompilerServices
{
    // https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.valuetaskawaiter-1
#if !POLYFILL_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    internal readonly struct ValueTaskAwaiter<TResult>
        : INotifyCompletion,
            ICriticalNotifyCompletion
    {
        private readonly ValueTask<TResult> _value;

        public ValueTaskAwaiter(ValueTask<TResult> value) => _value = value;

        public bool IsCompleted => _value.IsCompleted;

        public TResult GetResult() => _value.Result;

        public void OnCompleted(Action continuation) =>
            _value.AsTask().GetAwaiter().OnCompleted(continuation);

        public void UnsafeOnCompleted(Action continuation) =>
            _value.AsTask().GetAwaiter().UnsafeOnCompleted(continuation);
    }

    // https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.configuredvaluetaskawaitable-1
#if !POLYFILL_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    internal readonly struct ConfiguredValueTaskAwaitable<TResult>
    {
        private readonly ValueTask<TResult> _value;
        private readonly bool _continueOnCapturedContext;

        public ConfiguredValueTaskAwaitable(
            ValueTask<TResult> value,
            bool continueOnCapturedContext
        )
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
            private readonly ValueTask<TResult> _value;
            private readonly bool _continueOnCapturedContext;

            public Awaiter(ValueTask<TResult> value, bool continueOnCapturedContext)
            {
                _value = value;
                _continueOnCapturedContext = continueOnCapturedContext;
            }

            public bool IsCompleted => _value.IsCompleted;

            public TResult GetResult() => _value.Result;

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

    // https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.asyncvaluetaskmethodbuilder-1
#if !POLYFILL_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    internal struct AsyncValueTaskMethodBuilder<TResult>
    {
        private AsyncTaskMethodBuilder<TResult> _taskBuilder;

        public static AsyncValueTaskMethodBuilder<TResult> Create() =>
            new AsyncValueTaskMethodBuilder<TResult>
            {
                _taskBuilder = AsyncTaskMethodBuilder<TResult>.Create(),
            };

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine => _taskBuilder.Start(ref stateMachine);

        public void SetStateMachine(IAsyncStateMachine stateMachine) =>
            _taskBuilder.SetStateMachine(stateMachine);

        public void SetResult(TResult result) => _taskBuilder.SetResult(result);

        public void SetException(Exception exception) => _taskBuilder.SetException(exception);

        public ValueTask<TResult> Task => new ValueTask<TResult>(_taskBuilder.Task);

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
