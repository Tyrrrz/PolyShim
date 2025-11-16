#if FEATURE_TASK
#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Threading;
using System.Threading.Tasks;

internal static partial class PolyfillExtensions
{
    extension<T>(TaskCompletionSource<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskcompletionsource-1.setcanceled#system-threading-tasks-taskcompletionsource-1-setcanceled(system-threading-cancellationtoken)
        public void SetCanceled(CancellationToken cancellationToken)
        {
            if (!source.TrySetCanceled(cancellationToken))
                throw new InvalidOperationException(
                    "The task is already completed, canceled, or failed."
                );
        }
    }
}
#endif
#endif
