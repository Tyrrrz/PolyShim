#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net90_File
{
    // No file I/O on .NET Standard prior to 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
    extension(File)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.file.appendallbytes#system-io-file-appendallbytes(system-string-system-byte())
        public static void AppendAllBytes(string path, byte[] bytes)
        {
            using var stream = new FileStream(
                path,
                FileMode.Append,
                FileAccess.Write,
                FileShare.None
            );

            stream.Write(bytes, 0, bytes.Length);
        }

#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.file.appendallbytesasync#system-io-file-appendallbytesasync(system-string-system-byte()-system-threading-cancellationtoken)
        public static async Task AppendAllBytesAsync(
            string path,
            byte[] bytes,
            CancellationToken cancellationToken = default
        )
        {
            using var stream = new FileStream(
                path,
                FileMode.Append,
                FileAccess.Write,
                FileShare.None,
                4096,
                true
            );

            await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken)
                .ConfigureAwait(false);

            await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
#endif
    }
#endif
}
#endif
