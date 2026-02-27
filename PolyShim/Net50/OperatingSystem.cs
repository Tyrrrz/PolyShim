#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Runtime.InteropServices;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_OperatingSystem
{
    extension(OperatingSystem)
    {
        // Can only detect the platform on .NET Standard 1.3+
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.operatingsystem.isfreebsd
        public static bool IsFreeBSD() => RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);

        // https://learn.microsoft.com/dotnet/api/system.operatingsystem.islinux
        public static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        // https://learn.microsoft.com/dotnet/api/system.operatingsystem.ismacos
        public static bool IsMacOS() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        // https://learn.microsoft.com/dotnet/api/system.operatingsystem.iswindows
        public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        // Can only detect OS version on .NET Standard 2.0+ and .NET Core 2.0
#if (!NETSTANDARD || NETSTANDARD2_0_OR_GREATER) && (!NETCOREAPP || NETCOREAPP2_0_OR_GREATER)
        // https://learn.microsoft.com/dotnet/api/system.operatingsystem.iswindowsversionatleast
        public static bool IsWindowsVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
        {
            if (!IsWindows())
                return false;

            var version = Environment.OSVersion.Version;

            if (version.Major != major)
                return version.Major > major;

            if (version.Minor != minor)
                return version.Minor > minor;

            if (version.Build != build)
                return version.Build > build;

            return version.Revision >= revision;
        }
#endif
#endif
    }
}
#endif
