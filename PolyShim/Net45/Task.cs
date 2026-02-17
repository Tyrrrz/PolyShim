#if FEATURE_TASK && !NET45_OR_GREATER && !NETSTANDARD1_3_OR_GREATER && !NETCOREAPP
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net45_Task
{
    extension(Task)
    {
#if !(NETSTANDARD && !NETSTANDARD1_2_OR_GREATER)
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.delay#system-threading-tasks-task-delay(system-timespan-system-threading-cancellationtoken)
        public static Task Delay(TimeSpan delay, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            Timer? timer = null;
            timer = new Timer(
                _ =>
                {
                    timer?.Dispose();
                    tcs.TrySetResult(true);
                },
                null,
                delay,
                TimeSpan.FromMilliseconds(-1)
            );

            cancellationToken.Register(() =>
            {
                timer?.Dispose();
                tcs.TrySetCanceled();
            });

            return tcs.Task;
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.delay#system-threading-tasks-task-delay(system-timespan)
        public static Task Delay(TimeSpan delay) => Task.Delay(delay, CancellationToken.None);
#endif
    }
}
#endif
