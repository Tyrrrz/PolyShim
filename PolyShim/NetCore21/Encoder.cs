#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_Encoder
{
    extension(Encoder encoder)
    {
        // https://learn.microsoft.com/dotnet/api/system.text.encoder.convert#system-text-encoder-convert(system-readonlyspan((system-char))-system-span((system-byte))-system-boolean-system-int32@-system-int32@-system-boolean@)
        public void Convert(
            ReadOnlySpan<char> chars,
            Span<byte> bytes,
            bool flush,
            out int charsUsed,
            out int bytesUsed,
            out bool completed
        )
        {
            var charBuffer = chars.ToArray();
            var byteBuffer = new byte[bytes.Length];

            encoder.Convert(
                charBuffer,
                0,
                charBuffer.Length,
                byteBuffer,
                0,
                byteBuffer.Length,
                flush,
                out charsUsed,
                out bytesUsed,
                out completed
            );

            new ReadOnlySpan<byte>(byteBuffer, 0, bytesUsed).CopyTo(bytes);
        }
    }
}
#endif
