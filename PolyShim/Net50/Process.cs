#if FEATURE_PROCESS
#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

[ExcludeFromCodeCoverage]
internal static class _E5F55845EF964DA8B0A99B0086F4D357
{
#if FEATURE_TASK
    // https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.waitforexitasync
    public static async Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<object?>(
#if !(NETFRAMEWORK && !NET46_OR_GREATER)
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
#endif
}
#endif
#endif