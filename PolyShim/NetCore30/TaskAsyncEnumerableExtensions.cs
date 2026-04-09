#if !FEATURE_ASYNCINTERFACES
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Threading.Tasks;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
// https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskasyncenumerableextensions
internal static class MemberPolyfills_NetCore30_TaskAsyncEnumerableExtensions
{
    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskasyncenumerableextensions.withcancellation
        public ConfiguredCancelableAsyncEnumerable<T> WithCancellation(
            CancellationToken cancellationToken
        ) => new(source, continueOnCapturedContext: true, cancellationToken);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskasyncenumerableextensions.configureawait
        public ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait(
            bool continueOnCapturedContext
        ) => new(source, continueOnCapturedContext, cancellationToken: default);
    }
}
#endif
#endif
