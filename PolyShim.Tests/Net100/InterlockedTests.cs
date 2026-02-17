using System;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net100;

public class InterlockedTests
{
    private enum ByteEnum : byte
    {
        Value1 = 0b00001111,
        Value2 = 0b11110000,
        Value3 = 0b10101010,
    }

    private enum ShortEnum : short
    {
        Value1 = 0x0F0F,
        Value2 = unchecked((short)0xF0F0),
        Value3 = unchecked((short)0xAAAA),
    }

    private enum IntEnum : int
    {
        Value1 = 0x0000FFFF,
        Value2 = unchecked((int)0xFFFF0000),
        Value3 = unchecked((int)0xAAAAAAAA),
    }

    private enum LongEnum : long
    {
        Value1 = 0x00000000FFFFFFFF,
        Value2 = unchecked((long)0xFFFFFFFF00000000),
        Value3 = unchecked((long)0xAAAAAAAAAAAAAAAA),
    }

    [Theory]
    [InlineData((byte)0b00001111, (byte)0b11110000, (byte)0b00000000)]
    [InlineData((byte)0b11111111, (byte)0b10101010, (byte)0b10101010)]
    [InlineData((byte)0xFF, (byte)0x0F, (byte)0x0F)]
    [InlineData((byte)0, (byte)0xFF, (byte)0)]
    public void And_Byte_Test(byte initial, byte value, byte expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.And(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData((sbyte)0b00001111, (sbyte)0b01110000, (sbyte)0b00000000)]
    [InlineData((sbyte)0b01111111, (sbyte)0b00101010, (sbyte)0b00101010)]
    [InlineData((sbyte)-1, (sbyte)15, (sbyte)15)]
    [InlineData((sbyte)0, (sbyte)-1, (sbyte)0)]
    public void And_SByte_Test(sbyte initial, sbyte value, sbyte expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.And(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData((ushort)0x00FF, (ushort)0xFF00, (ushort)0x0000)]
    [InlineData((ushort)0xFFFF, (ushort)0xAAAA, (ushort)0xAAAA)]
    [InlineData((ushort)0x0FFF, (ushort)0xF0FF, (ushort)0x00FF)]
    [InlineData((ushort)0, (ushort)0xFFFF, (ushort)0)]
    public void And_UInt16_Test(ushort initial, ushort value, ushort expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.And(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData((short)0x00FF, (short)0x7F00, (short)0x0000)]
    [InlineData((short)0x7FFF, (short)0x2AAA, (short)0x2AAA)]
    [InlineData((short)-1, (short)255, (short)255)]
    [InlineData((short)0, (short)-1, (short)0)]
    public void And_Int16_Test(short initial, short value, short expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.And(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData(0x0000FFFF, unchecked((int)0xFFFF0000), 0x00000000)]
    [InlineData(unchecked((int)0xFFFFFFFF), unchecked((int)0xAAAAAAAA), unchecked((int)0xAAAAAAAA))]
    [InlineData(0x0FFFFFFF, unchecked((int)0xF0FFFFFF), 0x00FFFFFF)]
    [InlineData(0, -1, 0)]
    public void And_Int32_Test(int initial, int value, int expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.And(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData(0x0000FFFFU, 0xFFFF0000U, 0x00000000U)]
    [InlineData(0xFFFFFFFFU, 0xAAAAAAAAU, 0xAAAAAAAAU)]
    [InlineData(0x0FFFFFFFU, 0xF0FFFFFFU, 0x00FFFFFFU)]
    [InlineData(0U, 0xFFFFFFFFU, 0U)]
    public void And_UInt32_Test(uint initial, uint value, uint expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.And(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData(0x00000000FFFFFFFFL, unchecked((long)0xFFFFFFFF00000000L), 0x0000000000000000L)]
    [InlineData(
        unchecked((long)0xFFFFFFFFFFFFFFFFL),
        unchecked((long)0xAAAAAAAAAAAAAAAAL),
        unchecked((long)0xAAAAAAAAAAAAAAAAL)
    )]
    [InlineData(0x0FFFFFFFFFFFFFFFL, unchecked((long)0xF0FFFFFFFFFFFFFFL), 0x00FFFFFFFFFFFFFFL)]
    [InlineData(0L, -1L, 0L)]
    public void And_Int64_Test(long initial, long value, long expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.And(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData(0x00000000FFFFFFFFUL, 0xFFFFFFFF00000000UL, 0x0000000000000000UL)]
    [InlineData(0xFFFFFFFFFFFFFFFFUL, 0xAAAAAAAAAAAAAAAAUL, 0xAAAAAAAAAAAAAAAAUL)]
    [InlineData(0x0FFFFFFFFFFFFFFFUL, 0xF0FFFFFFFFFFFFFFUL, 0x00FFFFFFFFFFFFFFUL)]
    [InlineData(0UL, 0xFFFFFFFFFFFFFFFFUL, 0UL)]
    public void And_UInt64_Test(ulong initial, ulong value, ulong expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.And(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Fact]
    public void And_ByteEnum_Test()
    {
        // Arrange
        var location1 = ByteEnum.Value1;
        var location2 = ByteEnum.Value3;

        // Act
        var result1 = Interlocked.And(ref location1, ByteEnum.Value2);
        var result2 = Interlocked.And(ref location2, ByteEnum.Value2);

        // Assert
        result1.Should().Be(ByteEnum.Value1);
        location1.Should().Be((ByteEnum)0b00000000);

        result2.Should().Be(ByteEnum.Value3);
        location2.Should().Be((ByteEnum)0b10100000);
    }

    [Fact]
    public void And_IntEnum_Test()
    {
        // Arrange
        var location1 = IntEnum.Value1;
        var location2 = IntEnum.Value3;

        // Act
        var result1 = Interlocked.And(ref location1, IntEnum.Value2);
        var result2 = Interlocked.And(ref location2, IntEnum.Value2);

        // Assert
        result1.Should().Be(IntEnum.Value1);
        location1.Should().Be((IntEnum)0x00000000);

        result2.Should().Be(IntEnum.Value3);
        location2.Should().Be((IntEnum)unchecked((int)0xAAAA0000));
    }

    [Theory]
    [InlineData((byte)0b00001111, (byte)0b11110000, (byte)0b11111111)]
    [InlineData((byte)0b00000000, (byte)0b10101010, (byte)0b10101010)]
    [InlineData((byte)0xFF, (byte)0x0F, (byte)0xFF)]
    [InlineData((byte)0, (byte)0, (byte)0)]
    public void Or_Byte_Test(byte initial, byte value, byte expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.Or(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData((sbyte)0b00001111, (sbyte)0b01110000, (sbyte)0b01111111)]
    [InlineData((sbyte)0b00000000, (sbyte)0b00101010, (sbyte)0b00101010)]
    [InlineData((sbyte)-1, (sbyte)15, (sbyte)-1)]
    [InlineData((sbyte)0, (sbyte)0, (sbyte)0)]
    public void Or_SByte_Test(sbyte initial, sbyte value, sbyte expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.Or(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData((ushort)0x00FF, (ushort)0xFF00, (ushort)0xFFFF)]
    [InlineData((ushort)0x0000, (ushort)0xAAAA, (ushort)0xAAAA)]
    [InlineData((ushort)0x0FFF, (ushort)0xF000, (ushort)0xFFFF)]
    [InlineData((ushort)0, (ushort)0, (ushort)0)]
    public void Or_UInt16_Test(ushort initial, ushort value, ushort expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.Or(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData((short)0x00FF, (short)0x7F00, (short)0x7FFF)]
    [InlineData((short)0x0000, (short)0x2AAA, (short)0x2AAA)]
    [InlineData((short)0, (short)255, (short)255)]
    [InlineData((short)0, (short)0, (short)0)]
    public void Or_Int16_Test(short initial, short value, short expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.Or(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData(0x0000FFFF, unchecked((int)0xFFFF0000), unchecked((int)0xFFFFFFFF))]
    [InlineData(0x00000000, unchecked((int)0xAAAAAAAA), unchecked((int)0xAAAAAAAA))]
    [InlineData(0x0FFFFFFF, unchecked((int)0xF0000000), unchecked((int)0xFFFFFFFF))]
    [InlineData(0, 0, 0)]
    public void Or_Int32_Test(int initial, int value, int expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.Or(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData(0x0000FFFFU, 0xFFFF0000U, 0xFFFFFFFFU)]
    [InlineData(0x00000000U, 0xAAAAAAAAU, 0xAAAAAAAAU)]
    [InlineData(0x0FFFFFFFU, 0xF0000000U, 0xFFFFFFFFU)]
    [InlineData(0U, 0U, 0U)]
    public void Or_UInt32_Test(uint initial, uint value, uint expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.Or(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData(
        0x00000000FFFFFFFFL,
        unchecked((long)0xFFFFFFFF00000000L),
        unchecked((long)0xFFFFFFFFFFFFFFFFL)
    )]
    [InlineData(
        0x0000000000000000L,
        unchecked((long)0xAAAAAAAAAAAAAAAAL),
        unchecked((long)0xAAAAAAAAAAAAAAAAL)
    )]
    [InlineData(
        0x0FFFFFFFFFFFFFFFL,
        unchecked((long)0xF000000000000000L),
        unchecked((long)0xFFFFFFFFFFFFFFFFL)
    )]
    [InlineData(0L, 0L, 0L)]
    public void Or_Int64_Test(long initial, long value, long expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.Or(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Theory]
    [InlineData(0x00000000FFFFFFFFUL, 0xFFFFFFFF00000000UL, 0xFFFFFFFFFFFFFFFFUL)]
    [InlineData(0x0000000000000000UL, 0xAAAAAAAAAAAAAAAAUL, 0xAAAAAAAAAAAAAAAAUL)]
    [InlineData(0x0FFFFFFFFFFFFFFFUL, 0xF000000000000000UL, 0xFFFFFFFFFFFFFFFFUL)]
    [InlineData(0UL, 0UL, 0UL)]
    public void Or_UInt64_Test(ulong initial, ulong value, ulong expected)
    {
        // Arrange
        var location = initial;

        // Act
        var result = Interlocked.Or(ref location, value);

        // Assert
        result.Should().Be(initial);
        location.Should().Be(expected);
    }

    [Fact]
    public void Or_ByteEnum_Test()
    {
        // Arrange
        var location1 = ByteEnum.Value1;
        var location2 = ByteEnum.Value3;

        // Act
        var result1 = Interlocked.Or(ref location1, ByteEnum.Value2);
        var result2 = Interlocked.Or(ref location2, ByteEnum.Value2);

        // Assert
        result1.Should().Be(ByteEnum.Value1);
        location1.Should().Be((ByteEnum)0b11111111);

        result2.Should().Be(ByteEnum.Value3);
        location2.Should().Be((ByteEnum)0b11111010);
    }

    [Fact]
    public void Or_IntEnum_Test()
    {
        // Arrange
        var location1 = IntEnum.Value1;
        var location2 = IntEnum.Value3;

        // Act
        var result1 = Interlocked.Or(ref location1, IntEnum.Value2);
        var result2 = Interlocked.Or(ref location2, IntEnum.Value2);

        // Assert
        result1.Should().Be(IntEnum.Value1);
        location1.Should().Be((IntEnum)unchecked((int)0xFFFFFFFF));

        result2.Should().Be(IntEnum.Value3);
        location2.Should().Be((IntEnum)unchecked((int)0xFFFFAAAA));
    }

    [Fact]
    public void And_Float_ThrowsNotSupportedException()
    {
        // Arrange
        var location = 1.5f;

        // Act & Assert
        var ex = Assert.Throws<NotSupportedException>(() => Interlocked.And(ref location, 2.5f));
        ex.Message.Should().Contain("integer");
    }

    [Fact]
    public void Or_Float_ThrowsNotSupportedException()
    {
        // Arrange
        var location = 1.5f;

        // Act & Assert
        var ex = Assert.Throws<NotSupportedException>(() => Interlocked.Or(ref location, 2.5f));
        ex.Message.Should().Contain("integer");
    }

    [Fact]
    public void And_Double_ThrowsNotSupportedException()
    {
        // Arrange
        var location = 1.5;

        // Act & Assert
        var ex = Assert.Throws<NotSupportedException>(() => Interlocked.And(ref location, 2.5));
        ex.Message.Should().Contain("integer");
    }

    [Fact]
    public void Or_Double_ThrowsNotSupportedException()
    {
        // Arrange
        var location = 1.5;

        // Act & Assert
        var ex = Assert.Throws<NotSupportedException>(() => Interlocked.Or(ref location, 2.5));
        ex.Message.Should().Contain("integer");
    }
}
