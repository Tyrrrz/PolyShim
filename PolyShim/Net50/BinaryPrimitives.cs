#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_BinaryPrimitives
{
    extension(BinaryPrimitives)
    {
        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readsinglebigendian
        public static float ReadSingleBigEndian(ReadOnlySpan<byte> source)
        {
            var bits = BinaryPrimitives.ReadInt32BigEndian(source);
#if ALLOW_UNSAFE_BLOCKS
            unsafe
            {
                return *(float*)&bits;
            }
#else
            return BitConverter.ToSingle(BitConverter.GetBytes(bits), 0);
#endif
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readsinglelittleendian
        public static float ReadSingleLittleEndian(ReadOnlySpan<byte> source)
        {
            var bits = BinaryPrimitives.ReadInt32LittleEndian(source);
#if ALLOW_UNSAFE_BLOCKS
            unsafe
            {
                return *(float*)&bits;
            }
#else
            return BitConverter.ToSingle(BitConverter.GetBytes(bits), 0);
#endif
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readdoublebigendian
        public static double ReadDoubleBigEndian(ReadOnlySpan<byte> source) =>
            BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64BigEndian(source));

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readdoublelittleendian
        public static double ReadDoubleLittleEndian(ReadOnlySpan<byte> source) =>
            BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64LittleEndian(source));

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreadsinglebigendian
        public static bool TryReadSingleBigEndian(ReadOnlySpan<byte> source, out float value)
        {
            if (source.Length < sizeof(float))
            {
                value = default;
                return false;
            }

            value = BinaryPrimitives.ReadSingleBigEndian(source);
            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreadsinglelittleendian
        public static bool TryReadSingleLittleEndian(ReadOnlySpan<byte> source, out float value)
        {
            if (source.Length < sizeof(float))
            {
                value = default;
                return false;
            }

            value = BinaryPrimitives.ReadSingleLittleEndian(source);
            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreaddoublebigendian
        public static bool TryReadDoubleBigEndian(ReadOnlySpan<byte> source, out double value)
        {
            if (source.Length < sizeof(double))
            {
                value = default;
                return false;
            }

            value = BinaryPrimitives.ReadDoubleBigEndian(source);
            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreaddoublelittleendian
        public static bool TryReadDoubleLittleEndian(ReadOnlySpan<byte> source, out double value)
        {
            if (source.Length < sizeof(double))
            {
                value = default;
                return false;
            }

            value = BinaryPrimitives.ReadDoubleLittleEndian(source);
            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writesinglebigendian
        public static void WriteSingleBigEndian(Span<byte> destination, float value)
        {
#if ALLOW_UNSAFE_BLOCKS
            int bits;
            unsafe
            {
                bits = *(int*)&value;
            }
#else
            var bits = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
#endif
            BinaryPrimitives.WriteInt32BigEndian(destination, bits);
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writesinglelittleendian
        public static void WriteSingleLittleEndian(Span<byte> destination, float value)
        {
#if ALLOW_UNSAFE_BLOCKS
            int bits;
            unsafe
            {
                bits = *(int*)&value;
            }
#else
            var bits = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
#endif
            BinaryPrimitives.WriteInt32LittleEndian(destination, bits);
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writedoublebigendian
        public static void WriteDoubleBigEndian(Span<byte> destination, double value) =>
            BinaryPrimitives.WriteInt64BigEndian(
                destination,
                BitConverter.DoubleToInt64Bits(value)
            );

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writedoublelittleendian
        public static void WriteDoubleLittleEndian(Span<byte> destination, double value) =>
            BinaryPrimitives.WriteInt64LittleEndian(
                destination,
                BitConverter.DoubleToInt64Bits(value)
            );

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywritesinglebigendian
        public static bool TryWriteSingleBigEndian(Span<byte> destination, float value)
        {
            if (destination.Length < sizeof(float))
                return false;

            BinaryPrimitives.WriteSingleBigEndian(destination, value);
            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywritesinglelittleendian
        public static bool TryWriteSingleLittleEndian(Span<byte> destination, float value)
        {
            if (destination.Length < sizeof(float))
                return false;

            BinaryPrimitives.WriteSingleLittleEndian(destination, value);
            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywritedoublebigendian
        public static bool TryWriteDoubleBigEndian(Span<byte> destination, double value)
        {
            if (destination.Length < sizeof(double))
                return false;

            BinaryPrimitives.WriteDoubleBigEndian(destination, value);
            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywritedoublelittleendian
        public static bool TryWriteDoubleLittleEndian(Span<byte> destination, double value)
        {
            if (destination.Length < sizeof(double))
                return false;

            BinaryPrimitives.WriteDoubleLittleEndian(destination, value);
            return true;
        }
    }
}
#endif
