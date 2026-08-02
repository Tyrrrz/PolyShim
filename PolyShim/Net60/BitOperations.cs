#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net60_BitOperations
{
    extension(BitOperations)
    {
        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.ispow2#system-numerics-bitoperations-ispow2(system-int32)
        public static bool IsPow2(int value) => (value & (value - 1)) == 0 && value > 0;

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.ispow2#system-numerics-bitoperations-ispow2(system-uint32)
        public static bool IsPow2(uint value) => (value & (value - 1)) == 0 && value != 0;

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.ispow2#system-numerics-bitoperations-ispow2(system-int64)
        public static bool IsPow2(long value) => (value & (value - 1)) == 0 && value > 0;

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.ispow2#system-numerics-bitoperations-ispow2(system-uint64)
        public static bool IsPow2(ulong value) => (value & (value - 1)) == 0 && value != 0;

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.rounduptopowerof2#system-numerics-bitoperations-rounduptopowerof2(system-uint32)
        public static uint RoundUpToPowerOf2(uint value)
        {
            if (value == 0)
                return 0;

            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;

            return value + 1;
        }

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.rounduptopowerof2#system-numerics-bitoperations-rounduptopowerof2(system-uint64)
        public static ulong RoundUpToPowerOf2(ulong value)
        {
            if (value == 0)
                return 0;

            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            value |= value >> 32;

            return value + 1;
        }
    }
}
#endif
