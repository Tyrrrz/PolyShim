#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_File
{
    // No file I/O on .NET Standard prior to 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
    extension(File)
    {
#if FEATURE_TASK && FEATURE_ASYNCINTERFACES
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
#endif
}
#endif
