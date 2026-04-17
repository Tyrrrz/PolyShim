#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// Task is not available on all target frameworks within this TFM range without a NuGet package reference
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net90_Task
{
    extension(Task task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.wheneach#system-threading-tasks-task-wheneach(system-collections-generic-ienumerable((system-threading-tasks-task)))
        public static async IAsyncEnumerable<Task> WhenEach(IEnumerable<Task> tasks)
        {
            var remaining = new HashSet<Task>(tasks);
            while (remaining.Count > 0)
            {
                var completed = await Task.WhenAny(remaining).ConfigureAwait(false);
                remaining.Remove(completed);

                yield return completed;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.wheneach#system-threading-tasks-task-wheneach(system-threading-tasks-task())
        public static IAsyncEnumerable<Task> WhenEach(params Task[] tasks) =>
            WhenEach((IEnumerable<Task>)tasks);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.wheneach#system-threading-tasks-task-wheneach-1(system-collections-generic-ienumerable((system-threading-tasks-task((-0)))))
        public static async IAsyncEnumerable<Task<T>> WhenEach<T>(IEnumerable<Task<T>> tasks)
        {
            var remaining = new HashSet<Task<T>>(tasks);
            while (remaining.Count > 0)
            {
                var completed = await Task.WhenAny(remaining).ConfigureAwait(false);
                remaining.Remove(completed);

                yield return completed;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.wheneach#system-threading-tasks-task-wheneach-1(system-threading-tasks-task((-0))())
        public static IAsyncEnumerable<Task<T>> WhenEach<T>(params Task<T>[] tasks) =>
            WhenEach((IEnumerable<Task<T>>)tasks);
    }
}
#endif
#endif
