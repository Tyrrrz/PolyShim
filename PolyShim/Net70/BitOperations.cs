#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_BitOperations
{
    extension(BitOperations)
    {
        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.ispow2#system-numerics-bitoperations-ispow2(system-intptr)
        public static bool IsPow2(IntPtr value) =>
            IntPtr.Size == 4 ? BitOperations.IsPow2((int)value) : BitOperations.IsPow2((long)value);

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.ispow2#system-numerics-bitoperations-ispow2(system-uintptr)
        public static bool IsPow2(UIntPtr value) =>
            UIntPtr.Size == 4
                ? BitOperations.IsPow2((uint)value)
                : BitOperations.IsPow2((ulong)value);

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.rounduptopowerof2#system-numerics-bitoperations-rounduptopowerof2(system-uintptr)
        public static UIntPtr RoundUpToPowerOf2(UIntPtr value) =>
            UIntPtr.Size == 4
                ? (UIntPtr)BitOperations.RoundUpToPowerOf2((uint)value)
                : (UIntPtr)BitOperations.RoundUpToPowerOf2((ulong)value);

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.leadingzerocount#system-numerics-bitoperations-leadingzerocount(system-uintptr)
        public static int LeadingZeroCount(UIntPtr value) =>
            UIntPtr.Size == 4
                ? BitOperations.LeadingZeroCount((uint)value)
                : BitOperations.LeadingZeroCount((ulong)value);

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.log2#system-numerics-bitoperations-log2(system-uintptr)
        public static int Log2(UIntPtr value) =>
            UIntPtr.Size == 4 ? BitOperations.Log2((uint)value) : BitOperations.Log2((ulong)value);

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.popcount#system-numerics-bitoperations-popcount(system-uintptr)
        public static int PopCount(UIntPtr value) =>
            UIntPtr.Size == 4
                ? BitOperations.PopCount((uint)value)
                : BitOperations.PopCount((ulong)value);

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.trailingzerocount#system-numerics-bitoperations-trailingzerocount(system-intptr)
        public static int TrailingZeroCount(IntPtr value) =>
            IntPtr.Size == 4
                ? BitOperations.TrailingZeroCount((int)value)
                : BitOperations.TrailingZeroCount((long)value);

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.trailingzerocount#system-numerics-bitoperations-trailingzerocount(system-uintptr)
        public static int TrailingZeroCount(UIntPtr value) =>
            UIntPtr.Size == 4
                ? BitOperations.TrailingZeroCount((uint)value)
                : BitOperations.TrailingZeroCount((ulong)value);

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.rotateleft#system-numerics-bitoperations-rotateleft(system-uintptr-system-int32)
        public static UIntPtr RotateLeft(UIntPtr value, int offset) =>
            UIntPtr.Size == 4
                ? (UIntPtr)BitOperations.RotateLeft((uint)value, offset)
                : (UIntPtr)BitOperations.RotateLeft((ulong)value, offset);

        // https://learn.microsoft.com/dotnet/api/system.numerics.bitoperations.rotateright#system-numerics-bitoperations-rotateright(system-uintptr-system-int32)
        public static UIntPtr RotateRight(UIntPtr value, int offset) =>
            UIntPtr.Size == 4
                ? (UIntPtr)BitOperations.RotateRight((uint)value, offset)
                : (UIntPtr)BitOperations.RotateRight((ulong)value, offset);
    }
}
#endif
