#if FEATURE_PROCESS && FEATURE_MANAGEMENT
#if (NETCOREAPP1_0_OR_GREATER && !NETCOREAPP3_0_OR_GREATER) || (NET35_OR_GREATER) || (NETSTANDARD1_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management;

[ExcludeFromCodeCoverage]
internal static class _B51A2F7600DB4A4E851F782BA3C9FABA
{
    private static void KillProcessTree(int processId)
    {
        using var searcher = new ManagementObjectSearcher(
            $"SELECT * FROM Win32_Process WHERE ParentProcessID={processId}"
        );

        using var results = searcher.Get();

        // Kill the parent process
        try
        {
            using var proc = Process.GetProcessById(processId);
            if (!proc.HasExited)
                proc.Kill();
        }
        catch
        {
            // Do our best and ignore race conditions
        }

        // Kill the descendants
        foreach (var managementObject in results.Cast<ManagementObject>())
        {
            var childProcessId = Convert.ToInt32(managementObject["ProcessID"]);
            KillProcessTree(childProcessId);
        }
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.kill#system-diagnostics-process-kill(system-boolean)
    public static void Kill(this Process process, bool entireProcessTree)
    {
        if (!entireProcessTree)
        {
            process.Kill();
            return;
        }

        // Currently, this polyfill only supports Windows
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            process.Kill();
            return;
        }

        KillProcessTree(process.Id);
    }
}
#endif
#endif