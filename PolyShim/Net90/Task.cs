#if FEATURE_TASK
#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

internal static partial class PolyfillExtensions
{
#if FEATURE_ASYNCINTERFACES
    extension(Task task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.wheneach#system-threading-tasks-task-wheneach(system-collections-generic-ienumerable((system-threading-tasks-task)))
        public static async IAsyncEnumerable<Task> WhenEach(
            IEnumerable<Task> tasks,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            var remaining = new HashSet<Task>(tasks);
            while (remaining.Count > 0)
            {
                var completed = await Task.WhenAny(remaining).ConfigureAwait(false);
                remaining.Remove(completed);

                cancellationToken.ThrowIfCancellationRequested();
                yield return completed;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.wheneach#system-threading-tasks-task-wheneach(system-threading-tasks-task())
        public static IAsyncEnumerable<Task> WhenEach(params Task[] tasks) =>
            WhenEach(tasks, CancellationToken.None);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.wheneach#system-threading-tasks-task-wheneach-1(system-collections-generic-ienumerable((system-threading-tasks-task((-0)))))
        public static async IAsyncEnumerable<Task<T>> WhenEach<T>(
            IEnumerable<Task<T>> tasks,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            var remaining = new HashSet<Task<T>>(tasks);
            while (remaining.Count > 0)
            {
                var completed = await Task.WhenAny(remaining).ConfigureAwait(false);
                remaining.Remove(completed);

                cancellationToken.ThrowIfCancellationRequested();
                yield return completed;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.wheneach#system-threading-tasks-task-wheneach-1(system-threading-tasks-task((-0))())
        public static IAsyncEnumerable<Task<T>> WhenEach<T>(params Task<T>[] tasks) =>
            WhenEach(tasks, CancellationToken.None);
    }
#endif
}
#endif
#endif
