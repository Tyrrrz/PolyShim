#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net60_Environment
{
    extension(Environment)
    {
        // No Process class in .NET Standard 1.x
#if !NETSTANDARD || NETSTANDARD2_0_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.environment.processpath
        public static string? ProcessPath
        {
            get
            {
                try
                {
                    using var process = Process.GetCurrentProcess();
                    return process.MainModule?.FileName;
                }
                catch
                {
                    return null;
                }
            }
        }
#endif
    }
}
#endif
