#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_Environment
{
    extension(Environment)
    {
        // No Process class in .NET Standard 1.x
#if !NETSTANDARD || NETSTANDARD2_0_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.environment.processid
        public static int ProcessId
        {
            get
            {
                using var process = Process.GetCurrentProcess();
                return process.Id;
            }
        }
#endif
    }
}
#endif
