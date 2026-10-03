#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// Task is not available on all target frameworks within this TFM range without a NuGet package reference
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System;
#if NET40
// Microsoft.Bcl.Async exposes ConfiguredTaskAwaitable in a different namespace on net40.
using Microsoft.Runtime.CompilerServices;
#else
using System.Runtime.CompilerServices;
#endif
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net80_Task
{
#if NET40
    // DenyChildAttach was introduced in .NET Framework 4.5.
    private const TaskContinuationOptions DefaultContinuationOptions = TaskContinuationOptions.None;
#else
    private const TaskContinuationOptions DefaultContinuationOptions =
        TaskContinuationOptions.DenyChildAttach;
#endif

    extension(Task task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.configureawait#system-threading-tasks-task-configureawait(system-threading-tasks-configureawaitoptions)
        public ConfiguredTaskAwaitable ConfigureAwait(ConfigureAwaitOptions options)
        {
            if (
                (
                    options
                    & ~(
                        ConfigureAwaitOptions.ContinueOnCapturedContext
                        | ConfigureAwaitOptions.SuppressThrowing
                        | ConfigureAwaitOptions.ForceYielding
                    )
                ) != 0
            )
                throw new ArgumentOutOfRangeException(nameof(options));

            var suppressThrowing = (options & ConfigureAwaitOptions.SuppressThrowing) != 0;
            var forceYielding = (options & ConfigureAwaitOptions.ForceYielding) != 0;

            // SuppressThrowing and ForceYielding can't be expressed through the existing
            // ConfigureAwait(bool) overload, so they are emulated by projecting the task through
            // a continuation: observing (and discarding) the antecedent's exception instead of
            // propagating it for SuppressThrowing, and forcing the continuation to always run
            // on the thread pool, instead of potentially completing synchronously, for ForceYielding.
            var innerTask =
                suppressThrowing || forceYielding
                    ? task.ContinueWith(
                        t =>
                        {
                            if (!suppressThrowing)
                            {
                                t.GetAwaiter().GetResult();
                            }
                            else if (t.IsFaulted)
                            {
                                // Observe the exception so it doesn't surface as unobserved later on.
                                _ = t.Exception;
                            }
                        },
                        CancellationToken.None,
                        forceYielding
                            ? DefaultContinuationOptions
                            : TaskContinuationOptions.ExecuteSynchronously
                                | DefaultContinuationOptions,
                        TaskScheduler.Default
                    )
                    : task;

            return innerTask.ConfigureAwait(
                (options & ConfigureAwaitOptions.ContinueOnCapturedContext) != 0
            );
        }
    }

    extension<TResult>(Task<TResult> task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1.configureawait#system-threading-tasks-task-1-configureawait(system-threading-tasks-configureawaitoptions)
        public ConfiguredTaskAwaitable<TResult> ConfigureAwait(ConfigureAwaitOptions options)
        {
            if (
                (
                    options
                    & ~(
                        ConfigureAwaitOptions.ContinueOnCapturedContext
                        | ConfigureAwaitOptions.ForceYielding
                    )
                ) != 0
            )
            {
                // Mirrors the native implementation, which doesn't support suppressing exceptions
                // on a Task<TResult> because there's no sensible value to produce in that case.
                throw (options & ConfigureAwaitOptions.SuppressThrowing) == 0
                    ? new ArgumentOutOfRangeException(nameof(options))
                    : new ArgumentOutOfRangeException(
                        nameof(options),
                        "Task<TResult>.ConfigureAwait does not support ConfigureAwaitOptions.SuppressThrowing. "
                            + "To suppress throwing, instead cast the Task<TResult> to its base class Task and await that with SuppressThrowing."
                    );
            }

            var forceYielding = (options & ConfigureAwaitOptions.ForceYielding) != 0;

            // ForceYielding can't be expressed through the existing ConfigureAwait(bool) overload,
            // so it's emulated by forcing the continuation to always run on the thread pool, instead
            // of potentially completing synchronously.
            var innerTask = forceYielding
                ? task.ContinueWith(
                    t => t.GetAwaiter().GetResult(),
                    CancellationToken.None,
                    DefaultContinuationOptions,
                    TaskScheduler.Default
                )
                : task;

            return innerTask.ConfigureAwait(
                (options & ConfigureAwaitOptions.ContinueOnCapturedContext) != 0
            );
        }
    }
}
#endif
#endif
