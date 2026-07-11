#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_BinaryWriter
{
    extension(BinaryWriter writer)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.binarywriter.write#system-io-binarywriter-write(system-readonlyspan((system-byte)))
        public void Write(ReadOnlySpan<byte> buffer)
        {
            var bufferArray = buffer.ToArray();
            writer.Write(bufferArray, 0, bufferArray.Length);
        }

        // https://learn.microsoft.com/dotnet/api/system.io.binarywriter.write#system-io-binarywriter-write(system-readonlyspan((system-char)))
        public void Write(ReadOnlySpan<char> buffer)
        {
            var bufferArray = buffer.ToArray();
            writer.Write(bufferArray, 0, bufferArray.Length);
        }
    }
}
#endif
