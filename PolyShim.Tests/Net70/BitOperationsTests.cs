using System;
using System.Numerics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class BitOperationsTests
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    public void IsPow2_IntPtr_Test(int value, bool expected) =>
        BitOperations.IsPow2((IntPtr)value).Should().Be(expected);

    [Theory]
    [InlineData(0u, false)]
    [InlineData(1u, true)]
    [InlineData(1024u, true)]
    [InlineData(1023u, false)]
    public void IsPow2_UIntPtr_Test(uint value, bool expected) =>
        BitOperations.IsPow2((UIntPtr)value).Should().Be(expected);

    [Theory]
    [InlineData(0u, 0u)]
    [InlineData(3u, 4u)]
    [InlineData(1000u, 1024u)]
    public void RoundUpToPowerOf2_UIntPtr_Test(uint value, uint expected) =>
        BitOperations.RoundUpToPowerOf2((UIntPtr)value).Should().Be((UIntPtr)expected);

    [Fact]
    public void LeadingZeroCount_UIntPtr_Test()
    {
        // Arrange
        var bitWidth = UIntPtr.Size * 8;

        // Act & assert
        BitOperations.LeadingZeroCount((UIntPtr)0u).Should().Be(bitWidth);
        BitOperations.LeadingZeroCount((UIntPtr)1u).Should().Be(bitWidth - 1);
    }

    [Theory]
    [InlineData(1u, 0)]
    [InlineData(16u, 4)]
    public void Log2_UIntPtr_Test(uint value, int expected) =>
        BitOperations.Log2((UIntPtr)value).Should().Be(expected);

    [Theory]
    [InlineData(0u, 0)]
    [InlineData(7u, 3)]
    public void PopCount_UIntPtr_Test(uint value, int expected) =>
        BitOperations.PopCount((UIntPtr)value).Should().Be(expected);

    [Fact]
    public void TrailingZeroCount_IntPtr_Test()
    {
        // Arrange
        var bitWidth = UIntPtr.Size * 8;

        // Act & assert
        BitOperations.TrailingZeroCount((IntPtr)0).Should().Be(bitWidth);
        BitOperations.TrailingZeroCount((IntPtr)8).Should().Be(3);
    }

    [Fact]
    public void TrailingZeroCount_UIntPtr_Test()
    {
        // Arrange
        var bitWidth = UIntPtr.Size * 8;

        // Act & assert
        BitOperations.TrailingZeroCount((UIntPtr)0u).Should().Be(bitWidth);
        BitOperations.TrailingZeroCount((UIntPtr)8u).Should().Be(3);
    }

    [Fact]
    public void RotateLeft_UIntPtr_Test() =>
        BitOperations.RotateLeft((UIntPtr)0b_1000_0000u, 1).Should().Be((UIntPtr)0b_1_0000_0000u);

    [Fact]
    public void RotateRight_UIntPtr_Test() =>
        BitOperations.RotateRight((UIntPtr)0b_0000_0010u, 1).Should().Be((UIntPtr)0b_0000_0001u);

    [Fact]
    public void RotateRight_UIntPtr_FullCircle_Test()
    {
        // Arrange
        var bitWidth = UIntPtr.Size * 8;

        // Act & assert
        BitOperations.RotateRight((UIntPtr)1u, bitWidth).Should().Be((UIntPtr)1u);
    }
}
