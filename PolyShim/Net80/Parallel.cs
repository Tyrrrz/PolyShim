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
        ) =>
            await Parallel.ForEachAsync(
                Enumerable.Range(fromInclusive, toExclusive - fromInclusive),
                parallelOptions,
                // ValueTask conversion for newer targets
                async (i, ct) => await body(i, ct).ConfigureAwait(false)
            );

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
            ).ConfigureAwait(false);

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
