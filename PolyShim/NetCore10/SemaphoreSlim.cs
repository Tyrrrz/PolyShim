#if FEATURE_TASK
#if NETFRAMEWORK && !NET45_OR_GREATER
#nullable enable
#pragma warning disable CS0436

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_SemaphoreSlim
{
    extension(SemaphoreSlim semaphore)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.semaphoreslim.waitasync#system-threading-semaphoreslim-waitasync
        public Task WaitAsync()
        {
            if (semaphore.Wait(0))
            {
                return Task.CompletedTask;
            }

            return Task.Factory.StartNew(
                () => semaphore.Wait(),
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.Default
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.semaphoreslim.waitasync#system-threading-semaphoreslim-waitasync(system-threading-cancellationtoken)
        public Task WaitAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (semaphore.Wait(0, cancellationToken))
                {
                    return Task.CompletedTask;
                }
            }
            catch (OperationCanceledException)
            {
                return Task.FromCanceled(cancellationToken);
            }

            return Task.Factory.StartNew(
                () => semaphore.Wait(cancellationToken),
                cancellationToken,
                TaskCreationOptions.None,
                TaskScheduler.Default
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.semaphoreslim.waitasync#system-threading-semaphoreslim-waitasync(system-int32)
        public Task<bool> WaitAsync(int millisecondsTimeout) =>
            Task.Factory.StartNew(
                () => semaphore.Wait(millisecondsTimeout),
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.Default
            );

        // https://learn.microsoft.com/dotnet/api/system.threading.semaphoreslim.waitasync#system-threading-semaphoreslim-waitasync(system-int32-system-threading-cancellationtoken)
        public Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(
                () => semaphore.Wait(millisecondsTimeout, cancellationToken),
                cancellationToken,
                TaskCreationOptions.None,
                TaskScheduler.Default
            );

        // https://learn.microsoft.com/dotnet/api/system.threading.semaphoreslim.waitasync#system-threading-semaphoreslim-waitasync(system-timespan)
        public Task<bool> WaitAsync(TimeSpan timeout) =>
            Task.Factory.StartNew(
                () => semaphore.Wait(timeout),
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.Default
            );

        // https://learn.microsoft.com/dotnet/api/system.threading.semaphoreslim.waitasync#system-threading-semaphoreslim-waitasync(system-timespan-system-threading-cancellationtoken)
        public Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(
                () => semaphore.Wait(timeout, cancellationToken),
                cancellationToken,
                TaskCreationOptions.None,
                TaskScheduler.Default
            );
    }
}
#endif
#endif
