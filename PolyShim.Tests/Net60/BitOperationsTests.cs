using System.Numerics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class BitOperationsTests
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    [InlineData(-4, false)]
    public void IsPow2_Int32_Test(int value, bool expected) =>
        BitOperations.IsPow2(value).Should().Be(expected);

    [Theory]
    [InlineData(0u, false)]
    [InlineData(1u, true)]
    [InlineData(1024u, true)]
    [InlineData(1023u, false)]
    public void IsPow2_UInt32_Test(uint value, bool expected) =>
        BitOperations.IsPow2(value).Should().Be(expected);

    [Theory]
    [InlineData(0L, false)]
    [InlineData(1L, true)]
    [InlineData(2L, true)]
    [InlineData(-4L, false)]
    public void IsPow2_Int64_Test(long value, bool expected) =>
        BitOperations.IsPow2(value).Should().Be(expected);

    [Theory]
    [InlineData(0ul, false)]
    [InlineData(1ul, true)]
    [InlineData(1024ul, true)]
    [InlineData(1023ul, false)]
    public void IsPow2_UInt64_Test(ulong value, bool expected) =>
        BitOperations.IsPow2(value).Should().Be(expected);

    [Theory]
    [InlineData(0u, 0u)]
    [InlineData(1u, 1u)]
    [InlineData(2u, 2u)]
    [InlineData(3u, 4u)]
    [InlineData(5u, 8u)]
    [InlineData(1000u, 1024u)]
    public void RoundUpToPowerOf2_UInt32_Test(uint value, uint expected) =>
        BitOperations.RoundUpToPowerOf2(value).Should().Be(expected);

    [Fact]
    public void RoundUpToPowerOf2_UInt32_Overflow_Test() =>
        BitOperations.RoundUpToPowerOf2(uint.MaxValue).Should().Be(0u);

    [Theory]
    [InlineData(0ul, 0ul)]
    [InlineData(1ul, 1ul)]
    [InlineData(3ul, 4ul)]
    [InlineData(1000ul, 1024ul)]
    public void RoundUpToPowerOf2_UInt64_Test(ulong value, ulong expected) =>
        BitOperations.RoundUpToPowerOf2(value).Should().Be(expected);

    [Fact]
    public void RoundUpToPowerOf2_UInt64_Overflow_Test() =>
        BitOperations.RoundUpToPowerOf2(ulong.MaxValue).Should().Be(0ul);
}
