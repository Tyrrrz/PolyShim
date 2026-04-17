#if FEATURE_PROCESS
#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Process
{
    extension(Process process)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.waitforexit#system-diagnostics-process-waitforexit(system-timespan)
        public bool WaitForExit(TimeSpan timeout)
        {
            var totalMilliseconds = (long)timeout.TotalMilliseconds;
            if (totalMilliseconds < -1 || totalMilliseconds > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(timeout));

            return process.WaitForExit((int)totalMilliseconds);
        }
    }
}
#endif
#endif
