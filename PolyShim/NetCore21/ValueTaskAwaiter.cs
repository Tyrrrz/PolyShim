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

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.valuetaskawaiter
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ValueTaskAwaiter(ValueTask value)
    : INotifyCompletion,
        ICriticalNotifyCompletion
{
    public bool IsCompleted => value.IsCompleted;

    public void GetResult() => value.AsTask().GetAwaiter().GetResult();

    public void OnCompleted(Action continuation) =>
        value.AsTask().GetAwaiter().OnCompleted(continuation);

    public void UnsafeOnCompleted(Action continuation) =>
        value.AsTask().GetAwaiter().UnsafeOnCompleted(continuation);
}

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
