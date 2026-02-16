#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
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
internal static class MemberPolyfills_Net70_Stream
{
    extension(Stream stream)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.stream.readexactly#system-io-stream-readexactly(system-byte()-system-int32-system-int32)
        public void ReadExactly(byte[] buffer, int offset, int count)
        {
            var totalBytesRead = 0;
            while (totalBytesRead < count)
            {
                var bytesRead = stream.Read(
                    buffer,
                    offset + totalBytesRead,
                    count - totalBytesRead
                );

                if (bytesRead <= 0)
                    throw new EndOfStreamException();

                totalBytesRead += bytesRead;
            }
        }

#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.stream.readexactlyasync#system-io-stream-readexactlyasync(system-byte()-system-int32-system-int32-system-threading-cancellationtoken)
        public async Task ReadExactlyAsync(
            byte[] buffer,
            int offset,
            int count,
            CancellationToken cancellationToken = default
        )
        {
            var totalBytesRead = 0;
            while (totalBytesRead < count)
            {
                var bytesRead = await stream
                    .ReadAsync(
                        buffer,
                        offset + totalBytesRead,
                        count - totalBytesRead,
                        cancellationToken
                    )
                    .ConfigureAwait(false);

                if (bytesRead <= 0)
                    throw new EndOfStreamException();

                totalBytesRead += bytesRead;
            }
        }
#endif

        // https://learn.microsoft.com/dotnet/api/system.io.stream.readatleast
        public int ReadAtLeast(Span<byte> buffer, int minimumBytes, bool throwOnEndOfStream = true)
        {
            var totalBytesRead = 0;
            while (totalBytesRead < buffer.Length)
            {
                var bytesRead = stream.Read(buffer[totalBytesRead..]);
                if (bytesRead <= 0)
                    break;

                totalBytesRead += bytesRead;
            }

            if (totalBytesRead < minimumBytes && throwOnEndOfStream)
                throw new EndOfStreamException();

            return totalBytesRead;
        }

        // https://learn.microsoft.com/dotnet/api/system.io.stream.readexactly#system-io-stream-readexactly(system-span((system-byte)))
        public void ReadExactly(Span<byte> buffer)
        {
            var bufferArray = buffer.ToArray();
            stream.ReadExactly(bufferArray, 0, bufferArray.Length);
            bufferArray.CopyTo(buffer);
        }

#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.stream.readatleastasync
        public async Task<int> ReadAtLeastAsync(
            Memory<byte> buffer,
            int minimumBytes,
            bool throwOnEndOfStream = true,
            CancellationToken cancellationToken = default
        )
        {
            var totalBytesRead = 0;
            while (totalBytesRead < buffer.Length)
            {
                var bytesRead = await stream
                    .ReadAsync(buffer[totalBytesRead..], cancellationToken)
                    .ConfigureAwait(false);

                if (bytesRead <= 0)
                    break;

                totalBytesRead += bytesRead;
            }

            if (totalBytesRead < minimumBytes && throwOnEndOfStream)
                throw new EndOfStreamException();

            return totalBytesRead;
        }

        // https://learn.microsoft.com/dotnet/api/system.io.stream.readexactlyasync#system-io-stream-readexactlyasync(system-memory((system-byte))-system-threading-cancellationtoken)
        public async Task ReadExactlyAsync(
            Memory<byte> buffer,
            CancellationToken cancellationToken = default
        )
        {
            var bufferArray = buffer.ToArray();
            await stream
                .ReadExactlyAsync(bufferArray, 0, bufferArray.Length, cancellationToken)
                .ConfigureAwait(false);

            bufferArray.CopyTo(buffer);
        }
#endif
    }
}
#endif
