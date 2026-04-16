#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

// No file I/O on .NET Standard prior to 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER

using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net60_File
{
    extension(File)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.file.open#system-io-file-open(system-string-system-io-filestreamoptions)
        public static FileStream Open(string path, FileStreamOptions options)
        {
            var existed = File.Exists(path);

            var stream = new FileStream(
                path,
                options.Mode,
                options.Access,
                options.Share,
                options.BufferSize,
                options.Options
            );

            try
            {
#if !NETFRAMEWORK || NET40_OR_GREATER
                if (
                    !existed
                    && !OperatingSystem.IsWindows()
                    && options.UnixCreateMode is { } unixCreateMode
                    && options.Mode
                        is FileMode.CreateNew
                            or FileMode.Create
                            or FileMode.OpenOrCreate
                            or FileMode.Append
                )
                {
                    File.SetUnixFileMode(path, unixCreateMode & File.GetUnixFileMode(path));
                }
#endif
            }
            catch
            {
                stream.Dispose();
                throw;
            }

            return stream;
        }
    }
}
#endif
#endif
