#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Diagnostics.CodeAnalysis;

// No file I/O on .NET Standard prior to 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER

file static class NativeMethods
{
    // The stat struct on Unix is platform-specific; we read the raw bytes and extract the mode field.
    // 256 bytes is enough for any supported platform (Linux x86_64/arm64: 128–144 bytes; macOS: 144 bytes).
    [StructLayout(LayoutKind.Sequential)]
    public struct StatBuf
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] Data;
    }

    // Linux: stat is in libc.so.6
    [DllImport("libc", EntryPoint = "stat", SetLastError = true)]
    public static extern int StatLinux(string path, ref StatBuf buf);

    // macOS: the 64-bit inode variant is exported as stat$INODE64
    [DllImport("libSystem.dylib", EntryPoint = "stat$INODE64", SetLastError = true)]
    public static extern int StatMacOs(string path, ref StatBuf buf);

    [DllImport("libc", EntryPoint = "chmod", SetLastError = true)]
    public static extern int Chmod(string path, uint mode);

    public static int GetStat(string path, ref StatBuf buf) =>
        OperatingSystem.IsMacOS() ? StatMacOs(path, ref buf) : StatLinux(path, ref buf);

    // Returns the lower 12 bits (permission bits) of the native stat st_mode field.
    public static int ReadStatMode(StatBuf buf)
    {
        if (OperatingSystem.IsMacOS())
        {
            // macOS (all architectures): st_mode is uint16 at byte offset 4
            // struct stat { dev_t st_dev[4]; mode_t st_mode[2]; ... }
            return BitConverter.ToUInt16(buf.Data, 4);
        }

        // Linux: st_mode is uint32, but the offset differs by architecture.
        // x86_64: { dev_t[8] + ino_t[8] + nlink_t[8] + mode_t[4] } → offset 24
        // arm64:  { dev_t[8] + ino_t[8] + mode_t[4] + nlink_t[4] } → offset 16
#if FEATURE_RUNTIMEINFORMATION
        var offset = RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? 16 : 24;
#else
        var offset = 24; // Old targets ran on x86/x64 only
#endif
        return BitConverter.ToInt32(buf.Data, offset);
    }
}

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_File
{
    extension(File)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.file.getunixfilemode
        [UnsupportedOSPlatform("windows")]
        public static UnixFileMode GetUnixFileMode(string path)
        {
            if (OperatingSystem.IsWindows())
                throw new PlatformNotSupportedException();

            // Initialize the array so the marshaler has a non-null source for the in-direction copy.
            var buf = new NativeMethods.StatBuf { Data = new byte[256] };
            if (NativeMethods.GetStat(path, ref buf) != 0)
            {
                throw new IOException(
                    $"Could not get Unix file mode for '{path}' (errno={Marshal.GetLastWin32Error()})."
                );
            }

            return (UnixFileMode)(NativeMethods.ReadStatMode(buf) & 0xFFF);
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.setunixfilemode
        [UnsupportedOSPlatform("windows")]
        public static void SetUnixFileMode(string path, UnixFileMode mode)
        {
            if (OperatingSystem.IsWindows())
                throw new PlatformNotSupportedException();

            if (NativeMethods.Chmod(path, (uint)mode) != 0)
            {
                throw new IOException(
                    $"Could not set Unix file mode for '{path}' (errno={Marshal.GetLastWin32Error()})."
                );
            }
        }

#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.file.readlinesasync#system-io-file-readlinesasync(system-string-system-text-encoding-system-threading-cancellationtoken)
        public static async IAsyncEnumerable<string> ReadLinesAsync(
            string path,
            Encoding encoding,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                4096,
                FileOptions.Asynchronous | FileOptions.SequentialScan
            );

            using var reader = new StreamReader(stream, encoding);

            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
                if (line is not null)
                    yield return line;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.readlinesasync#system-io-file-readlinesasync(system-string-system-threading-cancellationtoken)
        public static async IAsyncEnumerable<string> ReadLinesAsync(
            string path,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                4096,
                FileOptions.Asynchronous | FileOptions.SequentialScan
            );

            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
                if (line is not null)
                    yield return line;
            }
        }
#endif
    }
}

#endif
#endif
