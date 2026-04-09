#if !FEATURE_ASYNCINTERFACES
#if FEATURE_TASK
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.configuredcancelableasyncenumerable-1
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ConfiguredCancelableAsyncEnumerable<T>(
    IAsyncEnumerable<T> enumerable,
    bool continueOnCapturedContext,
    CancellationToken cancellationToken
)
{
    public ConfiguredCancelableAsyncEnumerable<T> WithCancellation(CancellationToken ct) =>
        new(enumerable, continueOnCapturedContext, ct);

    public ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait(bool continueOnCapturedContext) =>
        new(enumerable, continueOnCapturedContext, cancellationToken);

    public Enumerator GetAsyncEnumerator() =>
        new(enumerable.GetAsyncEnumerator(cancellationToken), continueOnCapturedContext);

#if !POLYFILL_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    public readonly struct Enumerator(
        IAsyncEnumerator<T> enumerator,
        bool continueOnCapturedContext
    )
    {
        public T Current => enumerator.Current;

        public ConfiguredValueTaskAwaitable<bool> MoveNextAsync() =>
            enumerator.MoveNextAsync().ConfigureAwait(continueOnCapturedContext);

        public ConfiguredValueTaskAwaitable DisposeAsync() =>
            enumerator.DisposeAsync().ConfigureAwait(continueOnCapturedContext);
    }
}
#endif
#endif
