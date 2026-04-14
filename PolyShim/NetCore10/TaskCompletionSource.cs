#if FEATURE_TASK
#if (NETFRAMEWORK && !NET46_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_3_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_EXCLUDE_COVERAGE
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
