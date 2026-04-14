#if FEATURE_TASK
#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_TaskCompletionSourceOfT
{
    extension<T>(TaskCompletionSource<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskcompletionsource-1.setcanceled#system-threading-tasks-taskcompletionsource-1-setcanceled(system-threading-cancellationtoken)
        public void SetCanceled(CancellationToken cancellationToken)
        {
            if (!source.TrySetCanceled(cancellationToken))
            {
                throw new InvalidOperationException(
                    "The task is already completed, canceled, or failed."
                );
            }
        }
    }
}
#endif
#endif
