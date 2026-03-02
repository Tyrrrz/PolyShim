#if FEATURE_TASK
#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
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

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net60_Parallel
{
    extension(Parallel)
    {
        // Task instead of ValueTask for maximum compatibility
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel.foreachasync#system-threading-tasks-parallel-foreachasync-1(system-collections-generic-ienumerable((-0))-system-threading-tasks-paralleloptions-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask)))
        public static async Task ForEachAsync<T>(
            IEnumerable<T> source,
            ParallelOptions parallelOptions,
            Func<T, CancellationToken, Task> body
        )
        {
            using var semaphore = new SemaphoreSlim(
                parallelOptions.MaxDegreeOfParallelism switch
                {
                    > 0 => parallelOptions.MaxDegreeOfParallelism,
                    _ => Environment.ProcessorCount,
                }
            );

            var tasks = new List<Task>();
            foreach (var item in source)
            {
                tasks.Add(
                    Task.Factory.StartNew(
                            async () =>
                            {
#if !NETFRAMEWORK || NET45_OR_GREATER
                                await semaphore
                                    .WaitAsync(parallelOptions.CancellationToken)
                                    .ConfigureAwait(false);
#else
                                semaphore.Wait(parallelOptions.CancellationToken);
#endif
                                try
                                {
                                    await body(item, parallelOptions.CancellationToken)
                                        .ConfigureAwait(false);
                                }
                                finally
                                {
                                    semaphore.Release();
                                }
                            },
                            parallelOptions.CancellationToken,
                            TaskCreationOptions.None,
                            parallelOptions.TaskScheduler ?? TaskScheduler.Default
                        )
                        .Unwrap()
                );
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        // Task instead of ValueTask for maximum compatibility
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel.foreachasync#system-threading-tasks-parallel-foreachasync-1(system-collections-generic-ienumerable((-0))-system-threading-cancellationtoken-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask)))
        public static async Task ForEachAsync<T>(
            IEnumerable<T> source,
            CancellationToken cancellationToken,
            Func<T, CancellationToken, Task> body
        ) =>
            await ForEachAsync(
                    source,
                    new ParallelOptions { CancellationToken = cancellationToken },
                    body
                )
                .ConfigureAwait(false);

        // Task instead of ValueTask for maximum compatibility
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel.foreachasync#system-threading-tasks-parallel-foreachasync-1(system-collections-generic-ienumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask)))
        public static async Task ForEachAsync<T>(
            IEnumerable<T> source,
            Func<T, CancellationToken, Task> body
        ) => await ForEachAsync(source, CancellationToken.None, body).ConfigureAwait(false);

#if FEATURE_ASYNCINTERFACES
        // Task instead of ValueTask for maximum compatibility
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel.foreachasync#system-threading-tasks-parallel-foreachasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-tasks-paralleloptions-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask)))
        public static async Task ForEachAsync<T>(
            IAsyncEnumerable<T> source,
            ParallelOptions parallelOptions,
            Func<T, CancellationToken, Task> body
        )
        {
            using var semaphore = new SemaphoreSlim(
                parallelOptions.MaxDegreeOfParallelism switch
                {
                    > 0 => parallelOptions.MaxDegreeOfParallelism,
                    _ => Environment.ProcessorCount,
                }
            );

            var tasks = new List<Task>();

            await foreach (var item in source.WithCancellation(parallelOptions.CancellationToken))
            {
                tasks.Add(Task.Factory.StartNew(
                    async () =>
                    {
                        await semaphore.WaitAsync(parallelOptions.CancellationToken).ConfigureAwait(false);

                        try
                        {
                            await body(item, parallelOptions.CancellationToken).ConfigureAwait(false);
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    },
                    parallelOptions.CancellationToken,
                    TaskCreationOptions.None,
                    parallelOptions.TaskScheduler ?? TaskScheduler.Default
                ).Unwrap());
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        // Task instead of ValueTask for maximum compatibility
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel.foreachasync#system-threading-tasks-parallel-foreachasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-cancellationtoken-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask)))
        public static async Task ForEachAsync<T>(
            IAsyncEnumerable<T> source,
            CancellationToken cancellationToken,
            Func<T, CancellationToken, Task> body
        ) =>
            await ForEachAsync(
                source,
                new ParallelOptions { CancellationToken = cancellationToken },
                body
            ).ConfigureAwait(false);

        // Task instead of ValueTask for maximum compatibility
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel.foreachasync#system-threading-tasks-parallel-foreachasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask)))
        public static async Task ForEachAsync<T>(
            IAsyncEnumerable<T> source,
            Func<T, CancellationToken, Task> body
        ) => await ForEachAsync(source, CancellationToken.None, body).ConfigureAwait(false);
#endif
    }
}
#endif
#endif
