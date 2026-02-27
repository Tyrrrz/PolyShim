#if FEATURE_PROCESS
#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_Process
{
#if FEATURE_TASK
    extension(Process process)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.waitforexitasync
        public async Task WaitForExitAsync(CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<object?>(
#if !NETFRAMEWORK || NET46_OR_GREATER
                TaskCreationOptions.RunContinuationsAsynchronously
#endif
            );

            void HandleExited(object? sender, EventArgs args) => tcs.TrySetResult(null);

            try
            {
                process.EnableRaisingEvents = true;
            }
            // May throw if the process has already exited
            catch when(process.HasExited)
            {
                return;
            }

            process.Exited += HandleExited;

            try
            {
                using (cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken)))
                    await tcs.Task;
            }
            finally
            {
                process.Exited -= HandleExited;
            }
        }
    }
#endif
}
#endif
#endif
