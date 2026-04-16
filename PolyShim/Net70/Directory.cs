#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Directory
{
    // No file I/O on .NET Standard prior to 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
    [DllImport("libc", EntryPoint = "chmod", SetLastError = true)]
    private static extern int chmod(string path, uint mode);

    extension(Directory)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.directory.createdirectory#system-io-directory-createdirectory(system-string-system-io-unixfilemode)
        public static DirectoryInfo CreateDirectory(string path, UnixFileMode unixCreateMode)
        {
            if (OperatingSystem.IsWindows())
                throw new PlatformNotSupportedException();

            var info = Directory.CreateDirectory(path);

            if (chmod(path, (uint)unixCreateMode) != 0)
            {
                throw new IOException(
                    $"Could not set Unix file mode for '{path}' (errno={Marshal.GetLastWin32Error()})."
                );
            }

            return info;
        }
    }
#endif
}
#endif
