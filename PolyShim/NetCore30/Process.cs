#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NET35_OR_GREATER) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore30_Process
{
#if FEATURE_PROCESS && FEATURE_MANAGEMENT
    extension(Process process)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.kill#system-diagnostics-process-kill(system-boolean)
        public void Kill(bool entireProcessTree)
        {
            static void KillProcessTree(int processId)
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

            if (!entireProcessTree)
            {
                process.Kill();
                return;
            }

            // Currently, this polyfill only supports Windows
            if (!OperatingSystem.IsWindows())
            {
                process.Kill();
                return;
            }

            KillProcessTree(process.Id);
        }
    }
#endif
}
#endif
