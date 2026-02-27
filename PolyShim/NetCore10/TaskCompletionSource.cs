#if FEATURE_TASK
#if (NETFRAMEWORK && !NET46_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_3_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Threading;
using System.Threading.Tasks;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_TaskCompletionSource
{
    extension<T>(TaskCompletionSource<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskcompletionsource-1.trysetcanceled#system-threading-tasks-taskcompletionsource-1-trysetcanceled(system-threading-cancellationtoken)
        public bool TrySetCanceled(CancellationToken cancellationToken) =>
            source.TrySetException(new OperationCanceledException(cancellationToken));
    }
}
#endif
#endif
