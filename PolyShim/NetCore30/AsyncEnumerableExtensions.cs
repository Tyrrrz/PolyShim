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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore30_IAsyncEnumerable
{
    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskasyncenumerableextensions.withcancellation
        public ConfiguredCancelableAsyncEnumerable<T> WithCancellation(
            CancellationToken cancellationToken
        ) =>
            new ConfiguredCancelableAsyncEnumerable<T>(
                source,
                continueOnCapturedContext: true,
                cancellationToken
            );

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskasyncenumerableextensions.configureawait
        public ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait(
            bool continueOnCapturedContext
        ) =>
            new ConfiguredCancelableAsyncEnumerable<T>(
                source,
                continueOnCapturedContext,
                cancellationToken: default
            );
    }
}
#endif
#endif
#endif
