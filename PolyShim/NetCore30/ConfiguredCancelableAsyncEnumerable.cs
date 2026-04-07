#if !FEATURE_ASYNCINTERFACES
#if FEATURE_TASK
#if !(NETCOREAPP && !NETCOREAPP2_0_OR_GREATER)
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
        new ConfiguredCancelableAsyncEnumerable<T>(enumerable, continueOnCapturedContext, ct);

    public ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait(bool continueOnCapturedContext) =>
        new ConfiguredCancelableAsyncEnumerable<T>(
            enumerable,
            continueOnCapturedContext,
            cancellationToken
        );

    public Enumerator GetAsyncEnumerator() =>
        new Enumerator(enumerable.GetAsyncEnumerator(cancellationToken));

#if !POLYFILL_COVERAGE
    [ExcludeFromCodeCoverage]
#endif
    public readonly struct Enumerator(IAsyncEnumerator<T> enumerator)
    {
        public T Current => enumerator.Current;

        public ValueTask<bool> MoveNextAsync() => enumerator.MoveNextAsync();

        public ValueTask DisposeAsync() => enumerator.DisposeAsync();
    }
}
#endif
#endif
#endif
