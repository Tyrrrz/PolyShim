using System;
using System.Numerics;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class BitOperationsTests
{
    [Fact]
    public void IsPow2_IntPtr_Test()
    {
        // Act & assert
        BitOperations.IsPow2((IntPtr)0).Should().BeFalse();
        BitOperations.IsPow2((IntPtr)1).Should().BeTrue();
        BitOperations.IsPow2((IntPtr)2).Should().BeTrue();
        BitOperations.IsPow2((IntPtr)3).Should().BeFalse();
    }

    [Fact]
    public void IsPow2_UIntPtr_Test()
    {
        // Act & assert
        BitOperations.IsPow2((UIntPtr)0u).Should().BeFalse();
        BitOperations.IsPow2((UIntPtr)1u).Should().BeTrue();
        BitOperations.IsPow2((UIntPtr)1024u).Should().BeTrue();
        BitOperations.IsPow2((UIntPtr)1023u).Should().BeFalse();
    }

    [Fact]
    public void RoundUpToPowerOf2_UIntPtr_Test()
    {
        // Act & assert
        BitOperations.RoundUpToPowerOf2((UIntPtr)0u).Should().Be((UIntPtr)0u);
        BitOperations.RoundUpToPowerOf2((UIntPtr)3u).Should().Be((UIntPtr)4u);
        BitOperations.RoundUpToPowerOf2((UIntPtr)1000u).Should().Be((UIntPtr)1024u);
    }

    [Fact]
    public void LeadingZeroCount_UIntPtr_Test()
    {
        // Arrange
        var bitWidth = UIntPtr.Size * 8;

        // Act & assert
        BitOperations.LeadingZeroCount((UIntPtr)0u).Should().Be(bitWidth);
        BitOperations.LeadingZeroCount((UIntPtr)1u).Should().Be(bitWidth - 1);
    }

    [Fact]
    public void Log2_UIntPtr_Test()
    {
        // Act & assert
        BitOperations.Log2((UIntPtr)1u).Should().Be(0);
        BitOperations.Log2((UIntPtr)16u).Should().Be(4);
    }

    [Fact]
    public void PopCount_UIntPtr_Test()
    {
        // Act & assert
        BitOperations.PopCount((UIntPtr)0u).Should().Be(0);
        BitOperations.PopCount((UIntPtr)7u).Should().Be(3);
    }

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
