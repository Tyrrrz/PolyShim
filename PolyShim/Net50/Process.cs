#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// Process is not available on all target frameworks within this TFM range without a NuGet package reference.
#if FEATURE_PROCESS
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_Process
{
    // Task infrastructure is required for async method support.
#if FEATURE_TASK
    extension(Process process)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.waitforexitasync
        public async Task WaitForExitAsync(CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource(
#if !NETFRAMEWORK || NET46_OR_GREATER
                TaskCreationOptions.RunContinuationsAsynchronously
#endif
            );

            void OnExited(object? sender, EventArgs args) => tcs.TrySetResult();

            try
            {
                process.EnableRaisingEvents = true;
            }
            // May throw if the process has already exited
            catch when(process.HasExited)
            {
                return;
            }

            process.Exited += OnExited;

            try
            {
                using (cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken)))
                    await tcs.Task;
            }
            finally
            {
                process.Exited -= OnExited;
            }
        }
    }
#endif
}
#endif
#endif
