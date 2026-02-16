#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
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
        public async Task WriteAsync(
            ReadOnlyMemory<char> buffer,
            CancellationToken cancellationToken = default
        )
        {
            var bufferArray = buffer.ToArray();

            cancellationToken.ThrowIfCancellationRequested();
            await writer.WriteAsync(bufferArray, 0, bufferArray.Length).ConfigureAwait(false);
        }
#endif
    }
}
#endif
