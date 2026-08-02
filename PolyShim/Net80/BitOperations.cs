#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

file static class Crc32CTable
{
    public static readonly uint[] Values = Generate();

    private static uint[] Generate()
    {
        const uint polynomial = 0x82F6_3B78u;
        var table = new uint[256];

        for (var i = 0u; i < 256; i++)
        {
            var entry = i;

            for (var j = 0; j < 8; j++)
            {
                entry = (entry & 1) != 0 ? (entry >> 1) ^ polynomial : entry >> 1;
            }

            table[i] = entry;
        }

        return table;
    }
}

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net80_BitOperations
{
    extension(BitOperations)
    {
        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.crc32c#system-numerics-bitoperations-crc32c(system-uint32-system-byte)
        public static uint Crc32C(uint crc, byte data) =>
            Crc32CTable.Values[(byte)(crc ^ data)] ^ (crc >> 8);

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.crc32c#system-numerics-bitoperations-crc32c(system-uint32-system-uint16)
        public static uint Crc32C(uint crc, ushort data)
        {
            crc = BitOperations.Crc32C(crc, (byte)data);
            crc = BitOperations.Crc32C(crc, (byte)(data >> 8));

            return crc;
        }

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.crc32c#system-numerics-bitoperations-crc32c(system-uint32-system-uint32)
        public static uint Crc32C(uint crc, uint data)
        {
            crc = BitOperations.Crc32C(crc, (byte)data);
            crc = BitOperations.Crc32C(crc, (byte)(data >> 8));
            crc = BitOperations.Crc32C(crc, (byte)(data >> 16));
            crc = BitOperations.Crc32C(crc, (byte)(data >> 24));

            return crc;
        }

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.crc32c#system-numerics-bitoperations-crc32c(system-uint32-system-uint64)
        public static uint Crc32C(uint crc, ulong data) =>
            BitOperations.Crc32C(BitOperations.Crc32C(crc, (uint)data), (uint)(data >> 32));
    }
}
#endif
