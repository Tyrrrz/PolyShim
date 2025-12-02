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
#if !FEATURE_VALUETASK
using ValueTask = System.Threading.Tasks.Task;
#endif

internal static partial class PolyfillExtensions
{
    extension(Parallel)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.parallel.forasync#system-threading-tasks-parallel-forasync-1(-0-0-system-threading-tasks-paralleloptions-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask)))
        public static Task ForAsync(
            int fromInclusive,
            int toExclusive,
            ParallelOptions parallelOptions,
            Func<int, CancellationToken, ValueTask> body
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
                    await Task.Factory.StartNew(
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

            return Task.WhenAll(tasks);
        }
    }
}
#endif
#endif
