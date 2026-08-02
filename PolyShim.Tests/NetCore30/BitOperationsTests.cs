using System.Numerics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class BitOperationsTests
{
    [Fact]
    public void PopCount_UInt32_Test()
    {
        // Act & assert
        BitOperations.PopCount(0b_0000_0000u).Should().Be(0);
        BitOperations.PopCount(0b_0000_0001u).Should().Be(1);
        BitOperations.PopCount(0b_0000_0111u).Should().Be(3);
        BitOperations.PopCount(0b_1111_1111u).Should().Be(8);
        BitOperations.PopCount(uint.MaxValue).Should().Be(32);
    }

    [Fact]
    public void PopCount_UInt64_Test()
    {
        // Act & assert
        BitOperations.PopCount(0ul).Should().Be(0);
        BitOperations.PopCount(1ul).Should().Be(1);
        BitOperations.PopCount(0b_0000_0111ul).Should().Be(3);
        BitOperations.PopCount(ulong.MaxValue).Should().Be(64);
    }

    [Fact]
    public void LeadingZeroCount_UInt32_Test()
    {
        // Act & assert
        BitOperations.LeadingZeroCount(0u).Should().Be(32);
        BitOperations.LeadingZeroCount(1u).Should().Be(31);
        BitOperations.LeadingZeroCount(uint.MaxValue).Should().Be(0);
        BitOperations.LeadingZeroCount(0b_1000_0000_0000_0000_0000_0000_0000_0000u).Should().Be(0);
    }

    [Fact]
    public void LeadingZeroCount_UInt64_Test()
    {
        // Act & assert
        BitOperations.LeadingZeroCount(0ul).Should().Be(64);
        BitOperations.LeadingZeroCount(1ul).Should().Be(63);
        BitOperations.LeadingZeroCount(ulong.MaxValue).Should().Be(0);
    }

    [Fact]
    public void Log2_UInt32_Test()
    {
        // Act & assert
        BitOperations.Log2(0u).Should().Be(0);
        BitOperations.Log2(1u).Should().Be(0);
        BitOperations.Log2(2u).Should().Be(1);
        BitOperations.Log2(15u).Should().Be(3);
        BitOperations.Log2(16u).Should().Be(4);
        BitOperations.Log2(uint.MaxValue).Should().Be(31);
    }

    [Fact]
    public void Log2_UInt64_Test()
    {
        // Act & assert
        BitOperations.Log2(0ul).Should().Be(0);
        BitOperations.Log2(1ul).Should().Be(0);
        BitOperations.Log2(1024ul).Should().Be(10);
        BitOperations.Log2(ulong.MaxValue).Should().Be(63);
    }

    [Fact]
    public void TrailingZeroCount_Int32_Test()
    {
        // Act & assert
        BitOperations.TrailingZeroCount(0).Should().Be(32);
        BitOperations.TrailingZeroCount(1).Should().Be(0);
        BitOperations.TrailingZeroCount(8).Should().Be(3);
        BitOperations.TrailingZeroCount(-1).Should().Be(0);
    }

    [Fact]
    public void TrailingZeroCount_UInt32_Test()
    {
        // Act & assert
        BitOperations.TrailingZeroCount(0u).Should().Be(32);
        BitOperations.TrailingZeroCount(1u).Should().Be(0);
        BitOperations.TrailingZeroCount(8u).Should().Be(3);
        BitOperations.TrailingZeroCount(uint.MaxValue).Should().Be(0);
    }

    [Fact]
    public void TrailingZeroCount_Int64_Test()
    {
        // Act & assert
        BitOperations.TrailingZeroCount(0L).Should().Be(64);
        BitOperations.TrailingZeroCount(1L).Should().Be(0);
        BitOperations.TrailingZeroCount(-1L).Should().Be(0);
    }

    [Fact]
    public void TrailingZeroCount_UInt64_Test()
    {
        // Act & assert
        BitOperations.TrailingZeroCount(0ul).Should().Be(64);
        BitOperations.TrailingZeroCount(1ul).Should().Be(0);
        BitOperations.TrailingZeroCount(ulong.MaxValue).Should().Be(0);
    }

    [Fact]
    public void RotateLeft_UInt32_Test() =>
        BitOperations.RotateLeft(0b_1000_0000u, 1).Should().Be(0b_1_0000_0000u);

    [Fact]
    public void RotateLeft_UInt32_Overflow_Test() =>
        BitOperations
            .RotateLeft(0b_1000_0000_0000_0000_0000_0000_0000_0000u, 1)
            .Should()
            .Be(0b_0000_0000_0000_0000_0000_0000_0000_0001u);

    [Fact]
    public void RotateLeft_UInt64_Test() =>
        BitOperations.RotateLeft(0b_1000_0000ul, 1).Should().Be(0b_1_0000_0000ul);

    [Fact]
    public void RotateRight_UInt32_Test() =>
        BitOperations
            .RotateRight(0b_0000_0001u, 1)
            .Should()
            .Be(0b_1000_0000_0000_0000_0000_0000_0000_0000u);

    [Fact]
    public void RotateRight_UInt64_Test() =>
        BitOperations
            .RotateRight(0b_0000_0001ul, 1)
            .Should()
            .Be(
                0b_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000ul
            );
}
