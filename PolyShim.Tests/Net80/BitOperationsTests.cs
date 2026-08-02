using System.Numerics;
using System.Text;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class BitOperationsTests
{
    [Fact]
    public void Crc32C_Byte_KnownCheckValue_Test()
    {
        // Arrange
        var data = Encoding.ASCII.GetBytes("123456789");

        // Act
        var crc = uint.MaxValue;
        foreach (var b in data)
            crc = BitOperations.Crc32C(crc, b);
        crc = ~crc;

        // Assert
        // https://reveng.sourceforge.io/crc-catalogue/17plus.htm#crc.cat.crc-32-iscsi
        crc.Should().Be(0xE3069283u);
    }

    [Fact]
    public void Crc32C_UInt16_MatchesByteByByte_Test()
    {
        // Arrange
        ushort data = 0x1234;

        // Act
        var viaUInt16 = BitOperations.Crc32C(0u, data);

        var viaBytes = BitOperations.Crc32C(0u, (byte)data);
        viaBytes = BitOperations.Crc32C(viaBytes, (byte)(data >> 8));

        // Assert
        viaUInt16.Should().Be(viaBytes);
    }

    [Fact]
    public void Crc32C_UInt32_MatchesByteByByte_Test()
    {
        // Arrange
        uint data = 0x1234_5678u;

        // Act
        var viaUInt32 = BitOperations.Crc32C(0u, data);

        var viaBytes = BitOperations.Crc32C(0u, (byte)data);
        viaBytes = BitOperations.Crc32C(viaBytes, (byte)(data >> 8));
        viaBytes = BitOperations.Crc32C(viaBytes, (byte)(data >> 16));
        viaBytes = BitOperations.Crc32C(viaBytes, (byte)(data >> 24));

        // Assert
        viaUInt32.Should().Be(viaBytes);
    }

    [Fact]
    public void Crc32C_UInt64_MatchesUInt32Halves_Test()
    {
        // Arrange
        ulong data = 0x1234_5678_9ABC_DEF0ul;

        // Act
        var viaUInt64 = BitOperations.Crc32C(0u, data);

        var viaUInt32Halves = BitOperations.Crc32C(0u, (uint)data);
        viaUInt32Halves = BitOperations.Crc32C(viaUInt32Halves, (uint)(data >> 32));

        // Assert
        viaUInt64.Should().Be(viaUInt32Halves);
    }
}
