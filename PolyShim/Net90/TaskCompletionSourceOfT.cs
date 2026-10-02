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
internal static class MemberPolyfills_Net90_TaskCompletionSourceOfT
{
    extension<T>(TaskCompletionSource<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskcompletionsource-1.setfromtask
        public void SetFromTask(Task<T> completedTask)
        {
            if (!source.TrySetFromTask(completedTask))
            {
                throw new InvalidOperationException(
                    "The task is already completed, canceled, or failed."
                );
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskcompletionsource-1.trysetfromtask
        public bool TrySetFromTask(Task<T> completedTask)
        {
            ArgumentNullException.ThrowIfNull(completedTask);

            if (!completedTask.IsCompleted)
            {
                throw new ArgumentException(
                    "The task must already be completed.",
                    nameof(completedTask)
                );
            }

            return completedTask.Status switch
            {
                TaskStatus.RanToCompletion => source.TrySetResult(completedTask.Result),
                TaskStatus.Faulted => source.TrySetException(
                    completedTask.Exception!.InnerExceptions
                ),
                _ => source.TrySetCanceled(GetCancellationToken(completedTask)),
            };
        }
    }

    // Task does not expose the token that canceled it, so it needs to be recovered by observing
    // the task directly.
    private static CancellationToken GetCancellationToken(Task canceledTask)
    {
        try
        {
            canceledTask.GetAwaiter().GetResult();
        }
        catch (OperationCanceledException ex)
        {
            return ex.CancellationToken;
        }
        catch
        {
            // Ignore other exceptions; fall back to an empty token below.
        }

        return default;
    }
}
#endif
#endif
