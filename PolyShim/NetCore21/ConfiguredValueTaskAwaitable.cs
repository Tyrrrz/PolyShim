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

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.configuredvaluetaskawaitable
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ConfiguredValueTaskAwaitable(
    ValueTask value,
    bool continueOnCapturedContext
)
{
    public Awaiter GetAwaiter() => new Awaiter(value, continueOnCapturedContext);

#if !POLYFILL_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    public readonly struct Awaiter(ValueTask value, bool continueOnCapturedContext)
        : INotifyCompletion,
            ICriticalNotifyCompletion
    {
        public bool IsCompleted => value.IsCompleted;

        public void GetResult() => value.AsTask().GetAwaiter().GetResult();

        public void OnCompleted(Action continuation) =>
            value
                .AsTask()
                .ConfigureAwait(continueOnCapturedContext)
                .GetAwaiter()
                .OnCompleted(continuation);

        public void UnsafeOnCompleted(Action continuation) =>
            value
                .AsTask()
                .ConfigureAwait(continueOnCapturedContext)
                .GetAwaiter()
                .UnsafeOnCompleted(continuation);
    }
}

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.configuredvaluetaskawaitable-1
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ConfiguredValueTaskAwaitable<TResult>(
    ValueTask<TResult> value,
    bool continueOnCapturedContext
)
{
    public Awaiter GetAwaiter() => new Awaiter(value, continueOnCapturedContext);

#if !POLYFILL_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    public readonly struct Awaiter(ValueTask<TResult> value, bool continueOnCapturedContext)
        : INotifyCompletion,
            ICriticalNotifyCompletion
    {
        public bool IsCompleted => value.IsCompleted;

        public TResult GetResult() => value.Result;

        public void OnCompleted(Action continuation) =>
            value
                .AsTask()
                .ConfigureAwait(continueOnCapturedContext)
                .GetAwaiter()
                .OnCompleted(continuation);

        public void UnsafeOnCompleted(Action continuation) =>
            value
                .AsTask()
                .ConfigureAwait(continueOnCapturedContext)
                .GetAwaiter()
                .UnsafeOnCompleted(continuation);
    }
}
#endif
#endif
