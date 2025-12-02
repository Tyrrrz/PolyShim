#if FEATURE_TASK
#if (NETFRAMEWORK && !NET46_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_3_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal static partial class PolyfillExtensions
{
    private static readonly Task _completedTask = Task.FromResult(0);

    extension(Task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.completedtask
        public static Task CompletedTask => _completedTask;

#if NETFRAMEWORK && !NET45_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.fromresult
        public static Task<T?> FromResult<T>(T? result)
        {
            var tcs = new TaskCompletionSource<T?>();
            tcs.TrySetResult(result);

            return tcs.Task;
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run(system-action-system-threading-cancellationtoken)
        public static Task Run(Action action, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run(system-action)
        public static Task Run(Action action) =>
            Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run(system-func((system-threading-tasks-task))-system-threading-cancellationtoken)
        public static Task Run(Func<Task> function, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(function, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run(system-func((system-threading-tasks-task)))
        public static Task Run(Func<Task> function) =>
            Task.Factory.StartNew(function, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run-1(system-func((-0))-system-threading-cancellationtoken)
        public static Task<T> Run<T>(Func<T> function, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(function, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run-1(system-func((-0)))
        public static Task<T> Run<T>(Func<T> function) =>
            Task.Factory.StartNew(function, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run-1(system-func((system-threading-tasks-task((-0))))-system-threading-cancellationtoken)
        public static Task<T> Run<T>(Func<Task<T>> function, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(function, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run-1(system-func((system-threading-tasks-task((-0)))))
        public static Task<T> Run<T>(Func<Task<T>> function) =>
            Task.Factory.StartNew(function, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenall#system-threading-tasks-task-whenall(system-collections-generic-ienumerable((system-threading-tasks-task)))
        public static Task WhenAll(IEnumerable<Task> tasks) =>
            Task.Factory.ContinueWhenAll(tasks as Task[] ?? tasks.ToArray(), _ => { });

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenall#system-threading-tasks-task-whenall(system-threading-tasks-task())
        public static Task WhenAll(params Task[] tasks) => WhenAll((IEnumerable<Task>)tasks);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenall#system-threading-tasks-task-whenall-1(system-collections-generic-ienumerable((system-threading-tasks-task((-0)))))
        public static Task<T[]> WhenAll<T>(IEnumerable<Task<T>> tasks) =>
            Task.Factory.ContinueWhenAll(
                tasks as Task<T>[] ?? tasks.ToArray(),
                completedTasks => completedTasks.Select(t => t.Result).ToArray()
            );

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenall#system-threading-tasks-task-whenall-1(system-threading-tasks-task((-0))())
        public static Task<T[]> WhenAll<T>(params Task<T>[] tasks) =>
            WhenAll((IEnumerable<Task<T>>)tasks);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenany#system-threading-tasks-task-whenany(system-collections-generic-ienumerable((system-threading-tasks-task)))
        public static Task<Task> WhenAny(IEnumerable<Task> tasks) =>
            Task.Factory.ContinueWhenAny(tasks as Task[] ?? tasks.ToArray(), t => t);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenany#system-threading-tasks-task-whenany(system-threading-tasks-task())
        public static Task<Task> WhenAny(params Task[] tasks) => WhenAny((IEnumerable<Task>)tasks);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenany#system-threading-tasks-task-whenany-1(system-collections-generic-ienumerable((system-threading-tasks-task((-0)))))
        public static Task<Task<T>> WhenAny<T>(IEnumerable<Task<T>> tasks) =>
            Task.Factory.ContinueWhenAny(tasks as Task<T>[] ?? tasks.ToArray(), t => t);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenany#system-threading-tasks-task-whenany-1(system-threading-tasks-task((-0))())
        public static Task<Task<T>> WhenAny<T>(params Task<T>[] tasks) =>
            WhenAny((IEnumerable<Task<T>>)tasks);
#endif
    }
}
#endif
#endif
