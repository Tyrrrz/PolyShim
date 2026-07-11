#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_BinaryPrimitives
{
    extension(BinaryPrimitives)
    {
        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.reverseendianness#system-buffers-binary-binaryprimitives-reverseendianness(system-byte)
        public static byte ReverseEndianness(byte value) => value;

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.reverseendianness#system-buffers-binary-binaryprimitives-reverseendianness(system-sbyte)
        public static sbyte ReverseEndianness(sbyte value) => value;
    }
}
#endif
