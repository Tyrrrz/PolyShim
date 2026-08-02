using System.Numerics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class BitOperationsTests
{
    [Theory]
    [InlineData(0b_0000_0000u, 0)]
    [InlineData(0b_0000_0001u, 1)]
    [InlineData(0b_0000_0111u, 3)]
    [InlineData(0b_1111_1111u, 8)]
    [InlineData(uint.MaxValue, 32)]
    public void PopCount_UInt32_Test(uint value, int expected) =>
        BitOperations.PopCount(value).Should().Be(expected);

    [Theory]
    [InlineData(0ul, 0)]
    [InlineData(1ul, 1)]
    [InlineData(0b_0000_0111ul, 3)]
    [InlineData(ulong.MaxValue, 64)]
    public void PopCount_UInt64_Test(ulong value, int expected) =>
        BitOperations.PopCount(value).Should().Be(expected);

    [Theory]
    [InlineData(0u, 32)]
    [InlineData(1u, 31)]
    [InlineData(uint.MaxValue, 0)]
    [InlineData(0b_1000_0000_0000_0000_0000_0000_0000_0000u, 0)]
    public void LeadingZeroCount_UInt32_Test(uint value, int expected) =>
        BitOperations.LeadingZeroCount(value).Should().Be(expected);

    [Theory]
    [InlineData(0ul, 64)]
    [InlineData(1ul, 63)]
    [InlineData(ulong.MaxValue, 0)]
    public void LeadingZeroCount_UInt64_Test(ulong value, int expected) =>
        BitOperations.LeadingZeroCount(value).Should().Be(expected);

    [Theory]
    [InlineData(0u, 0)]
    [InlineData(1u, 0)]
    [InlineData(2u, 1)]
    [InlineData(15u, 3)]
    [InlineData(16u, 4)]
    [InlineData(uint.MaxValue, 31)]
    public void Log2_UInt32_Test(uint value, int expected) =>
        BitOperations.Log2(value).Should().Be(expected);

    [Theory]
    [InlineData(0ul, 0)]
    [InlineData(1ul, 0)]
    [InlineData(1024ul, 10)]
    [InlineData(ulong.MaxValue, 63)]
    public void Log2_UInt64_Test(ulong value, int expected) =>
        BitOperations.Log2(value).Should().Be(expected);

    [Theory]
    [InlineData(0, 32)]
    [InlineData(1, 0)]
    [InlineData(8, 3)]
    [InlineData(-1, 0)]
    public void TrailingZeroCount_Int32_Test(int value, int expected) =>
        BitOperations.TrailingZeroCount(value).Should().Be(expected);

    [Theory]
    [InlineData(0u, 32)]
    [InlineData(1u, 0)]
    [InlineData(8u, 3)]
    [InlineData(uint.MaxValue, 0)]
    public void TrailingZeroCount_UInt32_Test(uint value, int expected) =>
        BitOperations.TrailingZeroCount(value).Should().Be(expected);

    [Theory]
    [InlineData(0L, 64)]
    [InlineData(1L, 0)]
    [InlineData(-1L, 0)]
    public void TrailingZeroCount_Int64_Test(long value, int expected) =>
        BitOperations.TrailingZeroCount(value).Should().Be(expected);

    [Theory]
    [InlineData(0ul, 64)]
    [InlineData(1ul, 0)]
    [InlineData(ulong.MaxValue, 0)]
    public void TrailingZeroCount_UInt64_Test(ulong value, int expected) =>
        BitOperations.TrailingZeroCount(value).Should().Be(expected);

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
