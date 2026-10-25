#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_Encoding
{
    extension(Encoding encoding)
    {
        // https://learn.microsoft.com/dotnet/api/system.text.encoding.getbytes?system-text-encoding-getbytes(system-readonlyspan((system-char))-system-span((system-byte)))
        public int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes)
        {
            var buffer = Encoding.UTF8.GetBytes(chars.ToArray());
            if (buffer.Length > bytes.Length)
            {
                throw new ArgumentException("Destination buffer is too small.");
            }

            buffer.CopyTo(bytes);

            return buffer.Length;
        }

        // https://learn.microsoft.com/dotnet/api/system.text.encoding.getstring?system-text-encoding-getstring(system-readonlyspan((system-byte)))
        public string GetString(ReadOnlySpan<byte> bytes) => encoding.GetString(bytes.ToArray());
    }
}
#endif
