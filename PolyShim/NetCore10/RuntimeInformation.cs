#if !FEATURE_RUNTIMEINFORMATION
// No way to detect on .NET Standard lower than 1.3
#if !(NETSTANDARD && !NETSTANDARD1_3_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Runtime.InteropServices;

// https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.runtimeinformation
[ExcludeFromCodeCoverage]
internal static class RuntimeInformation
{
    public static bool IsOSPlatform(OSPlatform osPlatform)
    {
        if (osPlatform == OSPlatform.FreeBSD)
        {
            return
                Environment.OSVersion.Platform == PlatformID.Unix &&
                File.Exists("/usr/bin/true");
        }

        if (osPlatform == OSPlatform.Linux)
        {
            return
                Environment.OSVersion.Platform == PlatformID.Unix &&
                File.Exists("/proc/version");
        }

        if (osPlatform == OSPlatform.OSX)
        {
            return
                Environment.OSVersion.Platform == PlatformID.Unix &&
                File.Exists("/usr/bin/sw_vers");
        }

        if (osPlatform == OSPlatform.Windows)
        {
            return Environment.OSVersion.Platform is
                PlatformID.Win32NT or
                PlatformID.Win32S or
                PlatformID.Win32Windows;
        }

        return false;
    }
}
#endif
#endif