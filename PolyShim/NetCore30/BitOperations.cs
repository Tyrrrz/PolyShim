#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Numerics;

// https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class BitOperations
{
    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.popcount#system-numerics-bitoperations-popcount(system-uint32)
    public static int PopCount(uint value)
    {
        var count = 0;

        while (value != 0)
        {
            count++;
            value &= value - 1;
        }

        return count;
    }

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.popcount#system-numerics-bitoperations-popcount(system-uint64)
    public static int PopCount(ulong value)
    {
        var count = 0;

        while (value != 0)
        {
            count++;
            value &= value - 1;
        }

        return count;
    }

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.leadingzerocount#system-numerics-bitoperations-leadingzerocount(system-uint32)
    public static int LeadingZeroCount(uint value)
    {
        if (value == 0)
            return 32;

        var count = 0;
        while ((value & 0x8000_0000u) == 0)
        {
            value <<= 1;
            count++;
        }

        return count;
    }

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.leadingzerocount#system-numerics-bitoperations-leadingzerocount(system-uint64)
    public static int LeadingZeroCount(ulong value)
    {
        if (value == 0)
            return 64;

        var count = 0;
        while ((value & 0x8000_0000_0000_0000ul) == 0)
        {
            value <<= 1;
            count++;
        }

        return count;
    }

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.log2#system-numerics-bitoperations-log2(system-uint32)
    public static int Log2(uint value) => value == 0 ? 0 : 31 - LeadingZeroCount(value);

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.log2#system-numerics-bitoperations-log2(system-uint64)
    public static int Log2(ulong value) => value == 0 ? 0 : 63 - LeadingZeroCount(value);

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.trailingzerocount#system-numerics-bitoperations-trailingzerocount(system-int32)
    public static int TrailingZeroCount(int value) => TrailingZeroCount((uint)value);

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.trailingzerocount#system-numerics-bitoperations-trailingzerocount(system-uint32)
    public static int TrailingZeroCount(uint value)
    {
        if (value == 0)
            return 32;

        var count = 0;
        while ((value & 1) == 0)
        {
            value >>= 1;
            count++;
        }

        return count;
    }

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.trailingzerocount#system-numerics-bitoperations-trailingzerocount(system-int64)
    public static int TrailingZeroCount(long value) => TrailingZeroCount((ulong)value);

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.trailingzerocount#system-numerics-bitoperations-trailingzerocount(system-uint64)
    public static int TrailingZeroCount(ulong value)
    {
        if (value == 0)
            return 64;

        var count = 0;
        while ((value & 1) == 0)
        {
            value >>= 1;
            count++;
        }

        return count;
    }

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.rotateleft#system-numerics-bitoperations-rotateleft(system-uint32-system-int32)
    public static uint RotateLeft(uint value, int offset) =>
        (value << offset) | (value >> (32 - offset));

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.rotateleft#system-numerics-bitoperations-rotateleft(system-uint64-system-int32)
    public static ulong RotateLeft(ulong value, int offset) =>
        (value << offset) | (value >> (64 - offset));

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.rotateright#system-numerics-bitoperations-rotateright(system-uint32-system-int32)
    public static uint RotateRight(uint value, int offset) =>
        (value >> offset) | (value << (32 - offset));

    // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.rotateright#system-numerics-bitoperations-rotateright(system-uint64-system-int32)
    public static ulong RotateRight(ulong value, int offset) =>
        (value >> offset) | (value << (64 - offset));
}
#endif
