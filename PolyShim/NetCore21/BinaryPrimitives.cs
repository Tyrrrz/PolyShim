#if !NET5_0_OR_GREATER
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Buffers.Binary;

// https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class BinaryPrimitives
{
    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.reverseendianness#system-buffers-binary-binaryprimitives-reverseendianness(system-int16)
    public static short ReverseEndianness(short value) =>
        (short)(((value & 0x00FF) << 8) | ((value >> 8) & 0x00FF));

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.reverseendianness#system-buffers-binary-binaryprimitives-reverseendianness(system-uint16)
    public static ushort ReverseEndianness(ushort value) =>
        (ushort)(((value & 0x00FFU) << 8) | ((value >> 8) & 0x00FFU));

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.reverseendianness#system-buffers-binary-binaryprimitives-reverseendianness(system-int32)
    public static int ReverseEndianness(int value) => (int)ReverseEndianness((uint)value);

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.reverseendianness#system-buffers-binary-binaryprimitives-reverseendianness(system-uint32)
    public static uint ReverseEndianness(uint value) =>
        ((value & 0x000000FFU) << 24)
        | ((value & 0x0000FF00U) << 8)
        | ((value & 0x00FF0000U) >> 8)
        | ((value & 0xFF000000U) >> 24);

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.reverseendianness#system-buffers-binary-binaryprimitives-reverseendianness(system-int64)
    public static long ReverseEndianness(long value) => (long)ReverseEndianness((ulong)value);

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.reverseendianness#system-buffers-binary-binaryprimitives-reverseendianness(system-uint64)
    public static ulong ReverseEndianness(ulong value) =>
        ((ulong)ReverseEndianness((uint)(value & 0xFFFFFFFFUL)) << 32)
        | (ulong)ReverseEndianness((uint)(value >> 32));

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.reverseendianness#system-buffers-binary-binaryprimitives-reverseendianness(system-byte)
    public static byte ReverseEndianness(byte value) => value;

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.reverseendianness#system-buffers-binary-binaryprimitives-reverseendianness(system-sbyte)
    public static sbyte ReverseEndianness(sbyte value) => value;

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readint16bigendian
    public static short ReadInt16BigEndian(ReadOnlySpan<byte> source) =>
        (short)((source[0] << 8) | source[1]);

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readint16littleendian
    public static short ReadInt16LittleEndian(ReadOnlySpan<byte> source) =>
        (short)(source[0] | (source[1] << 8));

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readuint16bigendian
    public static ushort ReadUInt16BigEndian(ReadOnlySpan<byte> source) =>
        (ushort)((source[0] << 8) | source[1]);

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readuint16littleendian
    public static ushort ReadUInt16LittleEndian(ReadOnlySpan<byte> source) =>
        (ushort)(source[0] | (source[1] << 8));

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readint32bigendian
    public static int ReadInt32BigEndian(ReadOnlySpan<byte> source) =>
        (source[0] << 24) | (source[1] << 16) | (source[2] << 8) | source[3];

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readint32littleendian
    public static int ReadInt32LittleEndian(ReadOnlySpan<byte> source) =>
        source[0] | (source[1] << 8) | (source[2] << 16) | (source[3] << 24);

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readuint32bigendian
    public static uint ReadUInt32BigEndian(ReadOnlySpan<byte> source) =>
        ((uint)source[0] << 24)
        | ((uint)source[1] << 16)
        | ((uint)source[2] << 8)
        | (uint)source[3];

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readuint32littleendian
    public static uint ReadUInt32LittleEndian(ReadOnlySpan<byte> source) =>
        (uint)source[0]
        | ((uint)source[1] << 8)
        | ((uint)source[2] << 16)
        | ((uint)source[3] << 24);

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readint64bigendian
    public static long ReadInt64BigEndian(ReadOnlySpan<byte> source) =>
        ((long)source[0] << 56)
        | ((long)source[1] << 48)
        | ((long)source[2] << 40)
        | ((long)source[3] << 32)
        | ((long)source[4] << 24)
        | ((long)source[5] << 16)
        | ((long)source[6] << 8)
        | (long)source[7];

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readint64littleendian
    public static long ReadInt64LittleEndian(ReadOnlySpan<byte> source) =>
        (long)source[0]
        | ((long)source[1] << 8)
        | ((long)source[2] << 16)
        | ((long)source[3] << 24)
        | ((long)source[4] << 32)
        | ((long)source[5] << 40)
        | ((long)source[6] << 48)
        | ((long)source[7] << 56);

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readuint64bigendian
    public static ulong ReadUInt64BigEndian(ReadOnlySpan<byte> source) =>
        ((ulong)source[0] << 56)
        | ((ulong)source[1] << 48)
        | ((ulong)source[2] << 40)
        | ((ulong)source[3] << 32)
        | ((ulong)source[4] << 24)
        | ((ulong)source[5] << 16)
        | ((ulong)source[6] << 8)
        | (ulong)source[7];

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readuint64littleendian
    public static ulong ReadUInt64LittleEndian(ReadOnlySpan<byte> source) =>
        (ulong)source[0]
        | ((ulong)source[1] << 8)
        | ((ulong)source[2] << 16)
        | ((ulong)source[3] << 24)
        | ((ulong)source[4] << 32)
        | ((ulong)source[5] << 40)
        | ((ulong)source[6] << 48)
        | ((ulong)source[7] << 56);

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readsinglebigendian
    public static float ReadSingleBigEndian(ReadOnlySpan<byte> source)
    {
        var bits = ReadInt32BigEndian(source);
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
        var bits = ReadInt32LittleEndian(source);
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
        BitConverter.Int64BitsToDouble(ReadInt64BigEndian(source));

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.readdoublelittleendian
    public static double ReadDoubleLittleEndian(ReadOnlySpan<byte> source) =>
        BitConverter.Int64BitsToDouble(ReadInt64LittleEndian(source));

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreadint16bigendian
    public static bool TryReadInt16BigEndian(ReadOnlySpan<byte> source, out short value)
    {
        if (source.Length < sizeof(short))
        {
            value = default;
            return false;
        }

        value = ReadInt16BigEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreadint16littleendian
    public static bool TryReadInt16LittleEndian(ReadOnlySpan<byte> source, out short value)
    {
        if (source.Length < sizeof(short))
        {
            value = default;
            return false;
        }

        value = ReadInt16LittleEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreaduint16bigendian
    public static bool TryReadUInt16BigEndian(ReadOnlySpan<byte> source, out ushort value)
    {
        if (source.Length < sizeof(ushort))
        {
            value = default;
            return false;
        }

        value = ReadUInt16BigEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreaduint16littleendian
    public static bool TryReadUInt16LittleEndian(ReadOnlySpan<byte> source, out ushort value)
    {
        if (source.Length < sizeof(ushort))
        {
            value = default;
            return false;
        }

        value = ReadUInt16LittleEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreadint32bigendian
    public static bool TryReadInt32BigEndian(ReadOnlySpan<byte> source, out int value)
    {
        if (source.Length < sizeof(int))
        {
            value = default;
            return false;
        }

        value = ReadInt32BigEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreadint32littleendian
    public static bool TryReadInt32LittleEndian(ReadOnlySpan<byte> source, out int value)
    {
        if (source.Length < sizeof(int))
        {
            value = default;
            return false;
        }

        value = ReadInt32LittleEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreaduint32bigendian
    public static bool TryReadUInt32BigEndian(ReadOnlySpan<byte> source, out uint value)
    {
        if (source.Length < sizeof(uint))
        {
            value = default;
            return false;
        }

        value = ReadUInt32BigEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreaduint32littleendian
    public static bool TryReadUInt32LittleEndian(ReadOnlySpan<byte> source, out uint value)
    {
        if (source.Length < sizeof(uint))
        {
            value = default;
            return false;
        }

        value = ReadUInt32LittleEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreadint64bigendian
    public static bool TryReadInt64BigEndian(ReadOnlySpan<byte> source, out long value)
    {
        if (source.Length < sizeof(long))
        {
            value = default;
            return false;
        }

        value = ReadInt64BigEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreadint64littleendian
    public static bool TryReadInt64LittleEndian(ReadOnlySpan<byte> source, out long value)
    {
        if (source.Length < sizeof(long))
        {
            value = default;
            return false;
        }

        value = ReadInt64LittleEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreaduint64bigendian
    public static bool TryReadUInt64BigEndian(ReadOnlySpan<byte> source, out ulong value)
    {
        if (source.Length < sizeof(ulong))
        {
            value = default;
            return false;
        }

        value = ReadUInt64BigEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreaduint64littleendian
    public static bool TryReadUInt64LittleEndian(ReadOnlySpan<byte> source, out ulong value)
    {
        if (source.Length < sizeof(ulong))
        {
            value = default;
            return false;
        }

        value = ReadUInt64LittleEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.tryreadsinglebigendian
    public static bool TryReadSingleBigEndian(ReadOnlySpan<byte> source, out float value)
    {
        if (source.Length < sizeof(float))
        {
            value = default;
            return false;
        }

        value = ReadSingleBigEndian(source);
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

        value = ReadSingleLittleEndian(source);
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

        value = ReadDoubleBigEndian(source);
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

        value = ReadDoubleLittleEndian(source);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writeint16bigendian
    public static void WriteInt16BigEndian(Span<byte> destination, short value)
    {
        destination[0] = (byte)(value >> 8);
        destination[1] = (byte)value;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writeint16littleendian
    public static void WriteInt16LittleEndian(Span<byte> destination, short value)
    {
        destination[0] = (byte)value;
        destination[1] = (byte)(value >> 8);
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writeuint16bigendian
    public static void WriteUInt16BigEndian(Span<byte> destination, ushort value)
    {
        destination[0] = (byte)(value >> 8);
        destination[1] = (byte)value;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writeuint16littleendian
    public static void WriteUInt16LittleEndian(Span<byte> destination, ushort value)
    {
        destination[0] = (byte)value;
        destination[1] = (byte)(value >> 8);
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writeint32bigendian
    public static void WriteInt32BigEndian(Span<byte> destination, int value)
    {
        destination[0] = (byte)(value >> 24);
        destination[1] = (byte)(value >> 16);
        destination[2] = (byte)(value >> 8);
        destination[3] = (byte)value;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writeint32littleendian
    public static void WriteInt32LittleEndian(Span<byte> destination, int value)
    {
        destination[0] = (byte)value;
        destination[1] = (byte)(value >> 8);
        destination[2] = (byte)(value >> 16);
        destination[3] = (byte)(value >> 24);
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writeuint32bigendian
    public static void WriteUInt32BigEndian(Span<byte> destination, uint value)
    {
        destination[0] = (byte)(value >> 24);
        destination[1] = (byte)(value >> 16);
        destination[2] = (byte)(value >> 8);
        destination[3] = (byte)value;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writeuint32littleendian
    public static void WriteUInt32LittleEndian(Span<byte> destination, uint value)
    {
        destination[0] = (byte)value;
        destination[1] = (byte)(value >> 8);
        destination[2] = (byte)(value >> 16);
        destination[3] = (byte)(value >> 24);
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writeint64bigendian
    public static void WriteInt64BigEndian(Span<byte> destination, long value)
    {
        destination[0] = (byte)(value >> 56);
        destination[1] = (byte)(value >> 48);
        destination[2] = (byte)(value >> 40);
        destination[3] = (byte)(value >> 32);
        destination[4] = (byte)(value >> 24);
        destination[5] = (byte)(value >> 16);
        destination[6] = (byte)(value >> 8);
        destination[7] = (byte)value;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writeint64littleendian
    public static void WriteInt64LittleEndian(Span<byte> destination, long value)
    {
        destination[0] = (byte)value;
        destination[1] = (byte)(value >> 8);
        destination[2] = (byte)(value >> 16);
        destination[3] = (byte)(value >> 24);
        destination[4] = (byte)(value >> 32);
        destination[5] = (byte)(value >> 40);
        destination[6] = (byte)(value >> 48);
        destination[7] = (byte)(value >> 56);
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writeuint64bigendian
    public static void WriteUInt64BigEndian(Span<byte> destination, ulong value)
    {
        destination[0] = (byte)(value >> 56);
        destination[1] = (byte)(value >> 48);
        destination[2] = (byte)(value >> 40);
        destination[3] = (byte)(value >> 32);
        destination[4] = (byte)(value >> 24);
        destination[5] = (byte)(value >> 16);
        destination[6] = (byte)(value >> 8);
        destination[7] = (byte)value;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writeuint64littleendian
    public static void WriteUInt64LittleEndian(Span<byte> destination, ulong value)
    {
        destination[0] = (byte)value;
        destination[1] = (byte)(value >> 8);
        destination[2] = (byte)(value >> 16);
        destination[3] = (byte)(value >> 24);
        destination[4] = (byte)(value >> 32);
        destination[5] = (byte)(value >> 40);
        destination[6] = (byte)(value >> 48);
        destination[7] = (byte)(value >> 56);
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
        WriteInt32BigEndian(destination, bits);
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
        WriteInt32LittleEndian(destination, bits);
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writedoublebigendian
    public static void WriteDoubleBigEndian(Span<byte> destination, double value) =>
        WriteInt64BigEndian(destination, BitConverter.DoubleToInt64Bits(value));

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.writedoublelittleendian
    public static void WriteDoubleLittleEndian(Span<byte> destination, double value) =>
        WriteInt64LittleEndian(destination, BitConverter.DoubleToInt64Bits(value));

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywriteint16bigendian
    public static bool TryWriteInt16BigEndian(Span<byte> destination, short value)
    {
        if (destination.Length < sizeof(short))
            return false;

        WriteInt16BigEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywriteint16littleendian
    public static bool TryWriteInt16LittleEndian(Span<byte> destination, short value)
    {
        if (destination.Length < sizeof(short))
            return false;

        WriteInt16LittleEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywriteuint16bigendian
    public static bool TryWriteUInt16BigEndian(Span<byte> destination, ushort value)
    {
        if (destination.Length < sizeof(ushort))
            return false;

        WriteUInt16BigEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywriteuint16littleendian
    public static bool TryWriteUInt16LittleEndian(Span<byte> destination, ushort value)
    {
        if (destination.Length < sizeof(ushort))
            return false;

        WriteUInt16LittleEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywriteint32bigendian
    public static bool TryWriteInt32BigEndian(Span<byte> destination, int value)
    {
        if (destination.Length < sizeof(int))
            return false;

        WriteInt32BigEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywriteint32littleendian
    public static bool TryWriteInt32LittleEndian(Span<byte> destination, int value)
    {
        if (destination.Length < sizeof(int))
            return false;

        WriteInt32LittleEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywriteuint32bigendian
    public static bool TryWriteUInt32BigEndian(Span<byte> destination, uint value)
    {
        if (destination.Length < sizeof(uint))
            return false;

        WriteUInt32BigEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywriteuint32littleendian
    public static bool TryWriteUInt32LittleEndian(Span<byte> destination, uint value)
    {
        if (destination.Length < sizeof(uint))
            return false;

        WriteUInt32LittleEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywriteint64bigendian
    public static bool TryWriteInt64BigEndian(Span<byte> destination, long value)
    {
        if (destination.Length < sizeof(long))
            return false;

        WriteInt64BigEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywriteint64littleendian
    public static bool TryWriteInt64LittleEndian(Span<byte> destination, long value)
    {
        if (destination.Length < sizeof(long))
            return false;

        WriteInt64LittleEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywriteuint64bigendian
    public static bool TryWriteUInt64BigEndian(Span<byte> destination, ulong value)
    {
        if (destination.Length < sizeof(ulong))
            return false;

        WriteUInt64BigEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywriteuint64littleendian
    public static bool TryWriteUInt64LittleEndian(Span<byte> destination, ulong value)
    {
        if (destination.Length < sizeof(ulong))
            return false;

        WriteUInt64LittleEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywritesinglebigendian
    public static bool TryWriteSingleBigEndian(Span<byte> destination, float value)
    {
        if (destination.Length < sizeof(float))
            return false;

        WriteSingleBigEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywritesinglelittleendian
    public static bool TryWriteSingleLittleEndian(Span<byte> destination, float value)
    {
        if (destination.Length < sizeof(float))
            return false;

        WriteSingleLittleEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywritedoublebigendian
    public static bool TryWriteDoubleBigEndian(Span<byte> destination, double value)
    {
        if (destination.Length < sizeof(double))
            return false;

        WriteDoubleBigEndian(destination, value);
        return true;
    }

    // https://learn.microsoft.com/dotnet/api/system.buffers.binary.binaryprimitives.trywritedoublelittleendian
    public static bool TryWriteDoubleLittleEndian(Span<byte> destination, double value)
    {
        if (destination.Length < sizeof(double))
            return false;

        WriteDoubleLittleEndian(destination, value);
        return true;
    }
}
#endif
