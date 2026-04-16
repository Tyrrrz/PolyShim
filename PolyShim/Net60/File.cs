#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net60_File
{
    // No file I/O on .NET Standard prior to 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
    extension(File)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.file.open#system-io-file-open(system-string-system-io-filestreamoptions)
        public static FileStream Open(string path, FileStreamOptions options)
        {
            // Check existence before opening so we can skip chmod on pre-existing files
            // (matching the documented .NET 7 semantics where UnixCreateMode is only applied
            // when a new file is actually created).
            var existed = File.Exists(path);

            var stream = new FileStream(
                path,
                options.Mode,
                options.Access,
                options.Share,
                options.BufferSize,
                options.Options
            );

            // Honor UnixCreateMode on file-creation modes (best-effort; not applicable on Windows)
            try
            {
                if (
                    options.UnixCreateMode is { } unixCreateMode
                    && !OperatingSystem.IsWindows()
                    && !existed
                    && options.Mode
                        is FileMode.CreateNew
                            or FileMode.Create
                            or FileMode.OpenOrCreate
                            or FileMode.Append
                )
                {
                    File.SetUnixFileMode(path, unixCreateMode);
                }
            }
            catch
            {
                stream.Dispose();
                throw;
            }

            return stream;
        }
    }
#endif
}
#endif
