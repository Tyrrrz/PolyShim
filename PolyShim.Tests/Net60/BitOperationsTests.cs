using System.Numerics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class BitOperationsTests
{
    [Fact]
    public void IsPow2_Int32_Test()
    {
        // Act & assert
        BitOperations.IsPow2(0).Should().BeFalse();
        BitOperations.IsPow2(1).Should().BeTrue();
        BitOperations.IsPow2(2).Should().BeTrue();
        BitOperations.IsPow2(3).Should().BeFalse();
        BitOperations.IsPow2(-4).Should().BeFalse();
    }

    [Fact]
    public void IsPow2_UInt32_Test()
    {
        // Act & assert
        BitOperations.IsPow2(0u).Should().BeFalse();
        BitOperations.IsPow2(1u).Should().BeTrue();
        BitOperations.IsPow2(1024u).Should().BeTrue();
        BitOperations.IsPow2(1023u).Should().BeFalse();
    }

    [Fact]
    public void IsPow2_Int64_Test()
    {
        // Act & assert
        BitOperations.IsPow2(0L).Should().BeFalse();
        BitOperations.IsPow2(1L).Should().BeTrue();
        BitOperations.IsPow2(2L).Should().BeTrue();
        BitOperations.IsPow2(-4L).Should().BeFalse();
    }

    [Fact]
    public void IsPow2_UInt64_Test()
    {
        // Act & assert
        BitOperations.IsPow2(0ul).Should().BeFalse();
        BitOperations.IsPow2(1ul).Should().BeTrue();
        BitOperations.IsPow2(1024ul).Should().BeTrue();
        BitOperations.IsPow2(1023ul).Should().BeFalse();
    }

    [Fact]
    public void RoundUpToPowerOf2_UInt32_Test()
    {
        // Act & assert
        BitOperations.RoundUpToPowerOf2(0u).Should().Be(0u);
        BitOperations.RoundUpToPowerOf2(1u).Should().Be(1u);
        BitOperations.RoundUpToPowerOf2(2u).Should().Be(2u);
        BitOperations.RoundUpToPowerOf2(3u).Should().Be(4u);
        BitOperations.RoundUpToPowerOf2(5u).Should().Be(8u);
        BitOperations.RoundUpToPowerOf2(1000u).Should().Be(1024u);
    }

    [Fact]
    public void RoundUpToPowerOf2_UInt32_Overflow_Test() =>
        BitOperations.RoundUpToPowerOf2(uint.MaxValue).Should().Be(0u);

    [Fact]
    public void RoundUpToPowerOf2_UInt64_Test()
    {
        // Act & assert
        BitOperations.RoundUpToPowerOf2(0ul).Should().Be(0ul);
        BitOperations.RoundUpToPowerOf2(1ul).Should().Be(1ul);
        BitOperations.RoundUpToPowerOf2(3ul).Should().Be(4ul);
        BitOperations.RoundUpToPowerOf2(1000ul).Should().Be(1024ul);
    }

    [Fact]
    public void RoundUpToPowerOf2_UInt64_Overflow_Test() =>
        BitOperations.RoundUpToPowerOf2(ulong.MaxValue).Should().Be(0ul);
}
