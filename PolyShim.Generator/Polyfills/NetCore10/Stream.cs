#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Buffers;
using System.IO;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_Stream
{
    extension(Stream source)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.stream.copyto#system-io-stream-copyto(system-io-stream-system-int32)
        public void CopyTo(Stream destination, int bufferSize)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
            try
            {
                while (true)
                {
                    var bytesRead = source.Read(buffer, 0, buffer.Length);
                    if (bytesRead <= 0)
                        break;

                    destination.Write(buffer, 0, bytesRead);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.io.stream.copyto#system-io-stream-copyto(system-io-stream)
        public void CopyTo(Stream destination) => source.CopyTo(destination, 81920);
    }
}
#endif
