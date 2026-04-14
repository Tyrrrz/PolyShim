#if !FEATURE_ASYNCINTERFACES
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.asynciteratormethodbuilder
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal struct AsyncIteratorMethodBuilder
{
    private AsyncTaskMethodBuilder _methodBuilder;
    private bool _useBuilder;

    public static AsyncIteratorMethodBuilder Create() =>
        new() { _methodBuilder = AsyncTaskMethodBuilder.Create() };

    public void MoveNext<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine
    {
        if (_useBuilder)
            _methodBuilder.Start(ref stateMachine);
        else
            stateMachine.MoveNext();
    }

    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter,
        ref TStateMachine stateMachine
    )
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        _useBuilder = true;
        _methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);
    }

    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter,
        ref TStateMachine stateMachine
    )
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        _useBuilder = true;
        _methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
    }

    public void Complete() { }
}

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.asynciteratorstatemachineattribute
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
internal sealed class AsyncIteratorStateMachineAttribute(Type stateMachineType)
    : StateMachineAttribute(stateMachineType);
#endif
#endif
