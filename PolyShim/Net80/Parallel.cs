#if FEATURE_TASK
#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal static partial class PolyfillExtensions
{
    extension(Parallel)
    {
        // Task instead of ValueTask for maximum compatibility
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel.forasync#system-threading-tasks-parallel-forasync-1(-0-0-system-threading-tasks-paralleloptions-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask)))
        public static async Task ForAsync(
            int fromInclusive,
            int toExclusive,
            ParallelOptions parallelOptions,
            Func<int, CancellationToken, Task> body
        )
        {
            using var semaphore = new SemaphoreSlim(
                parallelOptions.MaxDegreeOfParallelism > 0
                    ? parallelOptions.MaxDegreeOfParallelism
                    : Environment.ProcessorCount
            );

            var tasks = Enumerable
                .Range(fromInclusive, toExclusive - fromInclusive)
                .Select(async i =>
                {
#if !NETFRAMEWORK || NET45_OR_GREATER
                    await semaphore
                        .WaitAsync(parallelOptions.CancellationToken)
                        .ConfigureAwait(false);
#else
                    await Task.Run(
                        () => semaphore.Wait(parallelOptions.CancellationToken),
                        parallelOptions.CancellationToken
                    ).ConfigureAwait(false);
#endif
                    try
                    {
                        await body(i, parallelOptions.CancellationToken).ConfigureAwait(false);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

            await Task.WhenAll(tasks);
        }

        // Task instead of ValueTask for maximum compatibility
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel.forasync#system-threading-tasks-parallel-forasync-1(-0-0-system-threading-cancellationtoken-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask)))
        public static async Task ForAsync(
            int fromInclusive,
            int toExclusive,
            CancellationToken cancellationToken,
            Func<int, CancellationToken, Task> body
        ) =>
            await ForAsync(
                fromInclusive,
                toExclusive,
                new ParallelOptions { CancellationToken = cancellationToken },
                body
            );

        // Task instead of ValueTask for maximum compatibility
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel.forasync#system-threading-tasks-parallel-forasync-1(-0-0-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask)))
        public static async Task ForAsync(
            int fromInclusive,
            int toExclusive,
            Func<int, CancellationToken, Task> body
        ) => await ForAsync(fromInclusive, toExclusive, CancellationToken.None, body);
    }
}
#endif
#endif
