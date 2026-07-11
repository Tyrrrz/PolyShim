#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_BinaryReader
{
    extension(BinaryReader reader)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.binaryreader.read#system-io-binaryreader-read(system-span((system-byte)))
        public int Read(Span<byte> buffer)
        {
            var bufferArray = new byte[buffer.Length];
            var result = reader.Read(bufferArray, 0, bufferArray.Length);
            bufferArray.AsSpan(0, result).CopyTo(buffer);

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.io.binaryreader.read#system-io-binaryreader-read(system-span((system-char)))
        public int Read(Span<char> buffer)
        {
            var bufferArray = new char[buffer.Length];
            var result = reader.Read(bufferArray, 0, bufferArray.Length);
            bufferArray.AsSpan(0, result).CopyTo(buffer);

            return result;
        }
    }
}
#endif
