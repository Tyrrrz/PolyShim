#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// Task is not available on all target frameworks within this TFM range without a NuGet package reference
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net90_TaskCompletionSource
{
    extension(TaskCompletionSource source)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskcompletionsource.trysetfromtask
        public bool TrySetFromTask(Task completedTask)
        {
            if (!completedTask.IsCompleted)
            {
                throw new ArgumentException(
                    "The task must already be completed.",
                    nameof(completedTask)
                );
            }

            if (completedTask.Status == TaskStatus.RanToCompletion)
                return source.TrySetResult();

            if (completedTask.Status == TaskStatus.Faulted)
                return source.TrySetException(completedTask.Exception!.InnerExceptions);

            // Task does not expose the token that canceled it, so it needs to be recovered
            // by observing the task directly. This is safe because the task is already known
            // to be completed at this point.
            var cancellationToken = new CancellationToken(true);
            try
            {
                completedTask.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException ex)
            {
                cancellationToken = ex.CancellationToken;
            }
            catch
            {
                // Ignore other exceptions; fall back to a plain canceled token above.
            }

            return source.TrySetCanceled(cancellationToken);
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskcompletionsource.setfromtask
        public void SetFromTask(Task completedTask)
        {
            if (!source.TrySetFromTask(completedTask))
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
