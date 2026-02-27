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
using System.Diagnostics.CodeAnalysis;

file static class TaskEx
{
    public static Task CompletedTask { get; } = Task.FromResult(0);
}

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_Task
{
    extension(Task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.completedtask
        public static Task CompletedTask => TaskEx.CompletedTask;

#if NETFRAMEWORK && !NET45_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.fromresult
        public static Task<T?> FromResult<T>(T? result)
        {
            var tcs = new TaskCompletionSource<T?>();
            tcs.TrySetResult(result);

            return tcs.Task;
        }
#endif

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.fromcanceled
        public static Task<T> FromCanceled<T>(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.TrySetCanceled(cancellationToken);
            return tcs.Task;
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.fromcanceled
        public static Task FromCanceled(CancellationToken cancellationToken) =>
            Task.FromCanceled<object>(cancellationToken);

#if NETFRAMEWORK && !NET45_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run(system-action-system-threading-cancellationtoken)
        public static Task Run(Action action, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(
                action,
                cancellationToken,
                TaskCreationOptions.None,
                TaskScheduler.Default
            );

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run(system-action)
        public static Task Run(Action action) => Task.Run(action, CancellationToken.None);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run(system-func((system-threading-tasks-task))-system-threading-cancellationtoken)
        public static Task Run(Func<Task> function, CancellationToken cancellationToken) =>
            Task
                .Factory.StartNew(
                    function,
                    cancellationToken,
                    TaskCreationOptions.None,
                    TaskScheduler.Default
                )
                .Unwrap();

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run(system-func((system-threading-tasks-task)))
        public static Task Run(Func<Task> function) => Task.Run(function, CancellationToken.None);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run-1(system-func((-0))-system-threading-cancellationtoken)
        public static Task<T> Run<T>(Func<T> function, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(
                function,
                cancellationToken,
                TaskCreationOptions.None,
                TaskScheduler.Default
            );

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run-1(system-func((-0)))
        public static Task<T> Run<T>(Func<T> function) =>
            Task.Run(function, CancellationToken.None);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run-1(system-func((system-threading-tasks-task((-0))))-system-threading-cancellationtoken)
        public static Task<T> Run<T>(Func<Task<T>> function, CancellationToken cancellationToken) =>
            Task
                .Factory.StartNew(
                    function,
                    cancellationToken,
                    TaskCreationOptions.None,
                    TaskScheduler.Default
                )
                .Unwrap();

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.run#system-threading-tasks-task-run-1(system-func((system-threading-tasks-task((-0)))))
        public static Task<T> Run<T>(Func<Task<T>> function) =>
            Task.Run(function, CancellationToken.None);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenall#system-threading-tasks-task-whenall(system-collections-generic-ienumerable((system-threading-tasks-task)))
        public static Task WhenAll(IEnumerable<Task> tasks) =>
            Task.Factory.ContinueWhenAll(
                tasks as Task[] ?? tasks.ToArray(),
                completedTasks =>
                {
                    foreach (var t in completedTasks)
                    {
                        t.GetAwaiter().GetResult();
                    }
                },
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default
            );

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenall#system-threading-tasks-task-whenall(system-threading-tasks-task())
        public static Task WhenAll(params Task[] tasks) => WhenAll((IEnumerable<Task>)tasks);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenall#system-threading-tasks-task-whenall-1(system-collections-generic-ienumerable((system-threading-tasks-task((-0)))))
        public static Task<T[]> WhenAll<T>(IEnumerable<Task<T>> tasks) =>
            Task.Factory.ContinueWhenAll(
                tasks as Task<T>[] ?? tasks.ToArray(),
                completedTasks => completedTasks.Select(t => t.GetAwaiter().GetResult()).ToArray(),
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default
            );

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenall#system-threading-tasks-task-whenall-1(system-threading-tasks-task((-0))())
        public static Task<T[]> WhenAll<T>(params Task<T>[] tasks) =>
            WhenAll((IEnumerable<Task<T>>)tasks);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenany#system-threading-tasks-task-whenany(system-collections-generic-ienumerable((system-threading-tasks-task)))
        public static Task<Task> WhenAny(IEnumerable<Task> tasks) =>
            Task.Factory.ContinueWhenAny(
                tasks as Task[] ?? tasks.ToArray(),
                t => t,
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default
            );

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenany#system-threading-tasks-task-whenany(system-threading-tasks-task())
        public static Task<Task> WhenAny(params Task[] tasks) => WhenAny((IEnumerable<Task>)tasks);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenany#system-threading-tasks-task-whenany-1(system-collections-generic-ienumerable((system-threading-tasks-task((-0)))))
        public static Task<Task<T>> WhenAny<T>(IEnumerable<Task<T>> tasks) =>
            Task.Factory.ContinueWhenAny(
                tasks as Task<T>[] ?? tasks.ToArray(),
                t => t,
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default
            );

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.whenany#system-threading-tasks-task-whenany-1(system-threading-tasks-task((-0))())
        public static Task<Task<T>> WhenAny<T>(params Task<T>[] tasks) =>
            WhenAny((IEnumerable<Task<T>>)tasks);
#endif

#if NETFRAMEWORK && !NET45_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.delay#system-threading-tasks-task-delay(system-timespan-system-threading-cancellationtoken)
        public static Task Delay(TimeSpan delay, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object?>();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            Timer? timer = null;
            CancellationTokenRegistration registration = default;

            void CleanupAndSetResult()
            {
                registration.Dispose();
                timer?.Dispose();
                tcs.TrySetResult(null);
            }

            void CleanupAndSetCanceled()
            {
                registration.Dispose();
                timer?.Dispose();
                tcs.TrySetCanceled();
            }

            timer = new Timer(
                _ => CleanupAndSetResult(),
                null,
                delay,
                TimeSpan.FromMilliseconds(-1)
            );

            registration = cancellationToken.Register(() => CleanupAndSetCanceled());

            return tcs.Task;
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.delay#system-threading-tasks-task-delay(system-timespan)
        public static Task Delay(TimeSpan delay) => Task.Delay(delay, CancellationToken.None);
#endif
    }
}
#endif
#endif
