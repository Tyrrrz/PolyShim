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

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.valuetaskawaiter-1
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTaskAwaiter<TResult>(ValueTask<TResult> value)
    : INotifyCompletion,
        ICriticalNotifyCompletion
{
    public bool IsCompleted => value.IsCompleted;

    public TResult GetResult() => value.Result;

    public void OnCompleted(Action continuation) =>
        value.AsTask().GetAwaiter().OnCompleted(continuation);

    public void UnsafeOnCompleted(Action continuation) =>
        value.AsTask().GetAwaiter().UnsafeOnCompleted(continuation);
}
#endif
#endif
