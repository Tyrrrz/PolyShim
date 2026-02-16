#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
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
