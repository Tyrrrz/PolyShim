#if !FEATURE_RUNTIMEINFORMATION
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif
using System.IO;

namespace System.Runtime.InteropServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.interopservices.runtimeinformation
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class RuntimeInformation
{
    // No way to detect on .NET Standard lower than 1.3
#if !(NETSTANDARD && !NETSTANDARD1_3_OR_GREATER)
    public static bool IsOSPlatform(OSPlatform osPlatform)
    {
        if (osPlatform == OSPlatform.FreeBSD)
        {
            return Environment.OSVersion.Platform == PlatformID.Unix
                && File.Exists("/usr/bin/true");
        }

        if (osPlatform == OSPlatform.Linux)
        {
            return Environment.OSVersion.Platform == PlatformID.Unix
                && File.Exists("/proc/version");
        }

        if (osPlatform == OSPlatform.OSX)
        {
            return Environment.OSVersion.Platform == PlatformID.Unix
                && File.Exists("/usr/bin/sw_vers");
        }

        if (osPlatform == OSPlatform.Windows)
        {
            return Environment.OSVersion.Platform
                is PlatformID.Win32NT
                    or PlatformID.Win32S
                    or PlatformID.Win32Windows;
        }

        return false;
    }
#endif
}
#endif
