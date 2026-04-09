#if FEATURE_TASK
// Compatibility package that provides FEATURE_VALUETASK doesn't backport this specific type on some target frameworks
#if !FEATURE_VALUETASK || (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
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
