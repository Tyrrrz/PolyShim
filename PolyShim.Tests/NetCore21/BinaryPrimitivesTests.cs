using System;
using System.Buffers.Binary;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class BinaryPrimitivesTests
{
    [Fact]
    public void ReverseEndianness_Int16_Test()
    {
        BinaryPrimitives.ReverseEndianness((short)0x1234).Should().Be(0x3412);
        BinaryPrimitives.ReverseEndianness((short)0x0000).Should().Be(0x0000);
        BinaryPrimitives.ReverseEndianness(unchecked((short)0xFF00)).Should().Be(0x00FF);
    }

    [Fact]
    public void ReverseEndianness_UInt16_Test()
    {
        BinaryPrimitives.ReverseEndianness((ushort)0x1234).Should().Be(0x3412);
        BinaryPrimitives.ReverseEndianness((ushort)0xFF00).Should().Be(0x00FF);
    }

    [Fact]
    public void ReverseEndianness_Int32_Test()
    {
        BinaryPrimitives.ReverseEndianness(0x12345678).Should().Be(0x78563412);
        BinaryPrimitives.ReverseEndianness(0x00000000).Should().Be(0x00000000);
        BinaryPrimitives.ReverseEndianness(unchecked((int)0xFF000000)).Should().Be(0x000000FF);
    }

    [Fact]
    public void ReverseEndianness_UInt32_Test()
    {
        BinaryPrimitives.ReverseEndianness(0x12345678U).Should().Be(0x78563412U);
        BinaryPrimitives.ReverseEndianness(0xFF000000U).Should().Be(0x000000FFU);
    }

    [Fact]
    public void ReverseEndianness_Int64_Test()
    {
        BinaryPrimitives.ReverseEndianness(0x0102030405060708L).Should().Be(0x0807060504030201L);
        BinaryPrimitives.ReverseEndianness(0L).Should().Be(0L);
    }

    [Fact]
    public void ReverseEndianness_UInt64_Test()
    {
        BinaryPrimitives.ReverseEndianness(0x0102030405060708UL).Should().Be(0x0807060504030201UL);
        BinaryPrimitives.ReverseEndianness(0UL).Should().Be(0UL);
    }

    [Fact]
    public void ReadInt16BigEndian_Test()
    {
        byte[] bytes = [0x12, 0x34, 0xFF];
        BinaryPrimitives.ReadInt16BigEndian(bytes).Should().Be(0x1234);
    }

    [Fact]
    public void ReadInt16LittleEndian_Test()
    {
        byte[] bytes = [0x34, 0x12, 0xFF];
        BinaryPrimitives.ReadInt16LittleEndian(bytes).Should().Be(0x1234);
    }

    [Fact]
    public void ReadUInt16BigEndian_Test()
    {
        byte[] bytes = [0xFF, 0x00];
        BinaryPrimitives.ReadUInt16BigEndian(bytes).Should().Be(0xFF00);
    }

    [Fact]
    public void ReadUInt16LittleEndian_Test()
    {
        byte[] bytes = [0x00, 0xFF];
        BinaryPrimitives.ReadUInt16LittleEndian(bytes).Should().Be(0xFF00);
    }

    [Fact]
    public void ReadInt32BigEndian_Test()
    {
        byte[] bytes = [0x12, 0x34, 0x56, 0x78];
        BinaryPrimitives.ReadInt32BigEndian(bytes).Should().Be(0x12345678);
    }

    [Fact]
    public void ReadInt32LittleEndian_Test()
    {
        byte[] bytes = [0x78, 0x56, 0x34, 0x12];
        BinaryPrimitives.ReadInt32LittleEndian(bytes).Should().Be(0x12345678);
    }

    [Fact]
    public void ReadUInt32BigEndian_Test()
    {
        byte[] bytes = [0xFF, 0x00, 0xFF, 0x00];
        BinaryPrimitives.ReadUInt32BigEndian(bytes).Should().Be(0xFF00FF00U);
    }

    [Fact]
    public void ReadUInt32LittleEndian_Test()
    {
        byte[] bytes = [0x00, 0xFF, 0x00, 0xFF];
        BinaryPrimitives.ReadUInt32LittleEndian(bytes).Should().Be(0xFF00FF00U);
    }

    [Fact]
    public void ReadInt64BigEndian_Test()
    {
        byte[] bytes = [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08];
        BinaryPrimitives.ReadInt64BigEndian(bytes).Should().Be(0x0102030405060708L);
    }

    [Fact]
    public void ReadInt64LittleEndian_Test()
    {
        byte[] bytes = [0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01];
        BinaryPrimitives.ReadInt64LittleEndian(bytes).Should().Be(0x0102030405060708L);
    }

    [Fact]
    public void ReadUInt64BigEndian_Test()
    {
        byte[] bytes = [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08];
        BinaryPrimitives.ReadUInt64BigEndian(bytes).Should().Be(0x0102030405060708UL);
    }

    [Fact]
    public void ReadUInt64LittleEndian_Test()
    {
        byte[] bytes = [0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01];
        BinaryPrimitives.ReadUInt64LittleEndian(bytes).Should().Be(0x0102030405060708UL);
    }

    [Fact]
    public void ReadSingleBigEndian_Test()
    {
        // 1.0f in IEEE 754: 0x3F800000
        byte[] bytes = [0x3F, 0x80, 0x00, 0x00];
        BinaryPrimitives.ReadSingleBigEndian(bytes).Should().Be(1.0f);
    }

    [Fact]
    public void ReadSingleLittleEndian_Test()
    {
        // 1.0f in IEEE 754: 0x3F800000 (little-endian bytes)
        byte[] bytes = [0x00, 0x00, 0x80, 0x3F];
        BinaryPrimitives.ReadSingleLittleEndian(bytes).Should().Be(1.0f);
    }

    [Fact]
    public void ReadDoubleBigEndian_Test()
    {
        // 1.0 in IEEE 754: 0x3FF0000000000000
        byte[] bytes = [0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
        BinaryPrimitives.ReadDoubleBigEndian(bytes).Should().Be(1.0);
    }

    [Fact]
    public void ReadDoubleLittleEndian_Test()
    {
        // 1.0 in IEEE 754: 0x3FF0000000000000 (little-endian bytes)
        byte[] bytes = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F];
        BinaryPrimitives.ReadDoubleLittleEndian(bytes).Should().Be(1.0);
    }

    [Fact]
    public void TryReadInt32BigEndian_TooShort_Test()
    {
        byte[] bytes = [0x01, 0x02];
        BinaryPrimitives.TryReadInt32BigEndian(bytes, out _).Should().BeFalse();
    }

    [Fact]
    public void TryReadInt32BigEndian_Success_Test()
    {
        byte[] bytes = [0x12, 0x34, 0x56, 0x78];
        BinaryPrimitives.TryReadInt32BigEndian(bytes, out var value).Should().BeTrue();
        value.Should().Be(0x12345678);
    }

    [Fact]
    public void TryReadInt64LittleEndian_TooShort_Test()
    {
        byte[] bytes = [0x01, 0x02, 0x03, 0x04];
        BinaryPrimitives.TryReadInt64LittleEndian(bytes, out _).Should().BeFalse();
    }

    [Fact]
    public void WriteInt16BigEndian_Test()
    {
        var buf = new byte[2];
        BinaryPrimitives.WriteInt16BigEndian(buf, 0x1234);
        buf.Should().Equal(0x12, 0x34);
    }

    [Fact]
    public void WriteInt16LittleEndian_Test()
    {
        var buf = new byte[2];
        BinaryPrimitives.WriteInt16LittleEndian(buf, 0x1234);
        buf.Should().Equal(0x34, 0x12);
    }

    [Fact]
    public void WriteInt32BigEndian_Test()
    {
        var buf = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(buf, 0x12345678);
        buf.Should().Equal(0x12, 0x34, 0x56, 0x78);
    }

    [Fact]
    public void WriteInt32LittleEndian_Test()
    {
        var buf = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(buf, 0x12345678);
        buf.Should().Equal(0x78, 0x56, 0x34, 0x12);
    }

    [Fact]
    public void WriteInt64BigEndian_Test()
    {
        var buf = new byte[8];
        BinaryPrimitives.WriteInt64BigEndian(buf, 0x0102030405060708L);
        buf.Should().Equal(0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08);
    }

    [Fact]
    public void WriteInt64LittleEndian_Test()
    {
        var buf = new byte[8];
        BinaryPrimitives.WriteInt64LittleEndian(buf, 0x0102030405060708L);
        buf.Should().Equal(0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01);
    }

    [Fact]
    public void WriteSingleBigEndian_Test()
    {
        var buf = new byte[4];
        BinaryPrimitives.WriteSingleBigEndian(buf, 1.0f);
        // 1.0f IEEE 754: 0x3F800000
        buf.Should().Equal(0x3F, 0x80, 0x00, 0x00);
    }

    [Fact]
    public void WriteSingleLittleEndian_Test()
    {
        var buf = new byte[4];
        BinaryPrimitives.WriteSingleLittleEndian(buf, 1.0f);
        // 1.0f IEEE 754 little-endian
        buf.Should().Equal(0x00, 0x00, 0x80, 0x3F);
    }

    [Fact]
    public void WriteDoubleBigEndian_Test()
    {
        var buf = new byte[8];
        BinaryPrimitives.WriteDoubleBigEndian(buf, 1.0);
        // 1.0 IEEE 754: 0x3FF0000000000000
        buf.Should().Equal(0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00);
    }

    [Fact]
    public void WriteDoubleLittleEndian_Test()
    {
        var buf = new byte[8];
        BinaryPrimitives.WriteDoubleLittleEndian(buf, 1.0);
        // 1.0 IEEE 754 little-endian
        buf.Should().Equal(0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F);
    }

    [Fact]
    public void TryWriteInt32BigEndian_TooShort_Test()
    {
        var buf = new byte[2];
        BinaryPrimitives.TryWriteInt32BigEndian(buf, 0x12345678).Should().BeFalse();
    }

    [Fact]
    public void TryWriteInt32BigEndian_Success_Test()
    {
        var buf = new byte[4];
        BinaryPrimitives.TryWriteInt32BigEndian(buf, 0x12345678).Should().BeTrue();
        buf.Should().Equal(0x12, 0x34, 0x56, 0x78);
    }

    [Fact]
    public void ReadWrite_RoundTrip_Int32BigEndian_Test()
    {
        var value = -123456789;
        var buf = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(buf, value);
        BinaryPrimitives.ReadInt32BigEndian(buf).Should().Be(value);
    }

    [Fact]
    public void ReadWrite_RoundTrip_DoubleBigEndian_Test()
    {
        var value = Math.PI;
        var buf = new byte[8];
        BinaryPrimitives.WriteDoubleBigEndian(buf, value);
        BinaryPrimitives.ReadDoubleBigEndian(buf).Should().Be(value);
    }
}
