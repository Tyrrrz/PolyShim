#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_TextReader
{
    extension(TextReader reader)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.textreader.read#system-io-textreader-read(system-span((system-char)))
        public int Read(Span<char> buffer)
        {
            var bufferArray = buffer.ToArray();
            var result = reader.Read(bufferArray, 0, bufferArray.Length);
            bufferArray.CopyTo(buffer);

            return result;
        }

        // Task infrastructure is required for async method support.
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.textreader.readasync#system-io-textreader-readasync(system-memory((system-char))-system-threading-cancellationtoken)
        public async ValueTask<int> ReadAsync(
            Memory<char> buffer,
            CancellationToken cancellationToken = default
        )
        {
            var bufferArray = buffer.ToArray();

            cancellationToken.ThrowIfCancellationRequested();

            var result = await reader
                .ReadAsync(bufferArray, 0, bufferArray.Length)
                .ConfigureAwait(false);

            bufferArray.CopyTo(buffer);

            return result;
        }
#endif
    }
}
#endif
