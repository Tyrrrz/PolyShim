#if !FEATURE_ASYNCINTERFACES
// Task infrastructure is required for async method return types
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.configuredcancelableasyncenumerable-1
#if !POLYSHIM_INCLUDE_COVERAGE
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

#if !POLYSHIM_INCLUDE_COVERAGE
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
