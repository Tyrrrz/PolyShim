#if !FEATURE_VALUETASK
#if FEATURE_TASK
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
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
#endif
#endif
