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

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_Stream
{
    extension(Stream stream)
    {
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.stream.copytoasync#system-io-stream-copytoasync(system-io-stream-system-threading-cancellationtoken)
        public async Task CopyToAsync(
            Stream destination,
            CancellationToken cancellationToken = default
        ) => await stream.CopyToAsync(destination, 81920, cancellationToken).ConfigureAwait(false);
#endif

        // https://learn.microsoft.com/dotnet/api/system.io.stream.read#system-io-stream-read(system-span((system-byte)))
        public int Read(Span<byte> buffer)
        {
            var bufferArray = buffer.ToArray();
            var result = stream.Read(bufferArray, 0, bufferArray.Length);
            bufferArray.CopyTo(buffer);

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.io.stream.write#system-io-stream-write(system-readonlyspan((system-byte)))
        public void Write(ReadOnlySpan<byte> buffer)
        {
            var bufferArray = buffer.ToArray();
            stream.Write(bufferArray, 0, bufferArray.Length);
        }

#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.stream.readasync#system-io-stream-readasync(system-memory((system-byte))-system-threading-cancellationtoken)
        public async Task<int> ReadAsync(
            Memory<byte> buffer,
            CancellationToken cancellationToken = default
        )
        {
            var bufferArray = buffer.ToArray();
            var result = await stream
                .ReadAsync(bufferArray, 0, bufferArray.Length, cancellationToken)
                .ConfigureAwait(false);

            bufferArray.CopyTo(buffer);

            return result;
        }

        // Task instead of ValueTask for maximum compatibility
        // https://learn.microsoft.com/dotnet/api/system.io.stream.writeasync#system-io-stream-writeasync(system-readonlymemory((system-byte))-system-threading-cancellationtoken)
        public async Task WriteAsync(
            ReadOnlyMemory<byte> buffer,
            CancellationToken cancellationToken = default
        )
        {
            var bufferArray = buffer.ToArray();
            await stream
                .WriteAsync(bufferArray, 0, bufferArray.Length, cancellationToken)
                .ConfigureAwait(false);
        }
#endif
    }
}
#endif
