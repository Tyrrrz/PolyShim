#if FEATURE_TASK
#if !FEATURE_VALUETASK
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.asyncvaluetaskmethodbuilder
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal struct AsyncValueTaskMethodBuilder
{
    private AsyncTaskMethodBuilder _taskBuilder;

    public static AsyncValueTaskMethodBuilder Create() =>
        new() { _taskBuilder = AsyncTaskMethodBuilder.Create() };

    public ValueTask Task => new(_taskBuilder.Task);

    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine => _taskBuilder.Start(ref stateMachine);

    public void SetStateMachine(IAsyncStateMachine stateMachine) =>
        _taskBuilder.SetStateMachine(stateMachine);

    public void SetResult() => _taskBuilder.SetResult();

    public void SetException(Exception exception) => _taskBuilder.SetException(exception);

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
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine =>
        _taskBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
}

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.asyncvaluetaskmethodbuilder-1
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal struct AsyncValueTaskMethodBuilder<TResult>
{
    private AsyncTaskMethodBuilder<TResult> _taskBuilder;

    public static AsyncValueTaskMethodBuilder<TResult> Create() =>
        new() { _taskBuilder = AsyncTaskMethodBuilder<TResult>.Create() };

    public ValueTask<TResult> Task => new(_taskBuilder.Task);

    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine => _taskBuilder.Start(ref stateMachine);

    public void SetStateMachine(IAsyncStateMachine stateMachine) =>
        _taskBuilder.SetStateMachine(stateMachine);

    public void SetResult(TResult result) => _taskBuilder.SetResult(result);

    public void SetException(Exception exception) => _taskBuilder.SetException(exception);

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
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine =>
        _taskBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
}
#endif
#endif
