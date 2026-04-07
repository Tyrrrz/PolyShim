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
#endif
#endif
