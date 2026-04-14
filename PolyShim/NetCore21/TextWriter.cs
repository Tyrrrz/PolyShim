#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_TextWriter
{
    extension(TextWriter writer)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.textwriter.write#system-io-textwriter-write(system-readonlyspan((system-char)))
        public void Write(ReadOnlySpan<char> buffer)
        {
            var bufferArray = buffer.ToArray();
            writer.Write(bufferArray, 0, bufferArray.Length);
        }

#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.textwriter.writeasync#system-io-textwriter-writeasync(system-readonlymemory((system-char))-system-threading-cancellationtoken)
        public async ValueTask WriteAsync(
            ReadOnlyMemory<char> buffer,
            CancellationToken cancellationToken = default
        )
        {
            var bufferArray = buffer.ToArray();

            cancellationToken.ThrowIfCancellationRequested();
            await writer.WriteAsync(bufferArray, 0, bufferArray.Length).ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.io.textwriter.writelineasync#system-io-textwriter-writelineasync(system-readonlymemory((system-char))-system-threading-cancellationtoken)
        public async ValueTask WriteLineAsync(
            ReadOnlyMemory<char> buffer,
            CancellationToken cancellationToken = default
        )
        {
            var bufferArray = buffer.ToArray();

            cancellationToken.ThrowIfCancellationRequested();
            await writer.WriteLineAsync(bufferArray, 0, bufferArray.Length).ConfigureAwait(false);
        }
#endif
    }
}
#endif
