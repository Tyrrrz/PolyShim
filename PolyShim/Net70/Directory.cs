#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Diagnostics.CodeAnalysis;

// No file I/O on .NET Standard prior to 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER

file static class NativeMethods
{
    [DllImport("libc", EntryPoint = "chmod", SetLastError = true)]
    public static extern int Chmod(string path, uint mode);
}

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Directory
{
    extension(Directory)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.directory.createdirectory#system-io-directory-createdirectory(system-string-system-io-unixfilemode)
        [UnsupportedOSPlatform("windows")]
        public static DirectoryInfo CreateDirectory(string path, UnixFileMode unixCreateMode)
        {
            if (OperatingSystem.IsWindows())
                throw new PlatformNotSupportedException();

            var existed = Directory.Exists(path);
            var info = Directory.CreateDirectory(path);

            if (!existed)
            {
                if (NativeMethods.Chmod(info.FullName, (uint)unixCreateMode) != 0)
                {
                    throw new IOException(
                        $"Could not set Unix file mode for '{path}' (errno={Marshal.GetLastWin32Error()})."
                    );
                }
            }

            return info;
        }

        // https://learn.microsoft.com/dotnet/api/system.io.directory.createtempsubdirectory#system-io-directory-createtempsubdirectory(system-string)
        public static DirectoryInfo CreateTempSubdirectory(string? prefix = null)
        {
            var tempPath = Path.GetTempPath();

            for (var attempt = 0; attempt < 10; attempt++)
            {
                var path = Path.Combine(tempPath, prefix + Path.GetRandomFileName());

                try
                {
                    return Directory.CreateDirectory(path);
                }
                catch (IOException) when (Directory.Exists(path))
                {
                    // Collision with an existing directory, try again with a different name
                }
            }

            throw new IOException("Failed to create a unique temporary subdirectory.");
        }
    }
}

#endif
#endif
