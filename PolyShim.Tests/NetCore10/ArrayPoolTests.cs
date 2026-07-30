using System;
using System.Buffers;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class ArrayPoolTests
{
    [Fact]
    public void Rent_ReturnsArray_WithAtLeastRequestedLength()
    {
        // Act
        var array = ArrayPool<int>.Shared.Rent(10);

        // Assert
        array.Length.Should().BeGreaterThanOrEqualTo(10);
    }

    [Fact]
    public void Rent_NegativeLength_Throws()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ArrayPool<int>.Shared.Rent(-1));
    }

    [Fact]
    public void Return_ThenRent_ReusesSameArray()
    {
        // Arrange
        var pool = ArrayPool<int>.Shared;
        var array = pool.Rent(16);

        // Act
        pool.Return(array);
        var reused = pool.Rent(16);

        // Assert
        reused.Should().BeSameAs(array);
    }

    [Fact]
    public void Return_ClearArray_ZeroesContents()
    {
        // Arrange
        var pool = ArrayPool<int>.Shared;
        var array = pool.Rent(4);
        array[0] = 42;
        array[1] = 99;

        // Act
        pool.Return(array, clearArray: true);
        var reused = pool.Rent(4);

        // Assert
        reused.Should().BeSameAs(array);
        reused[0].Should().Be(0);
        reused[1].Should().Be(0);
    }

    [Fact]
    public void Rent_SmallerThanCached_ReturnsSuitableArray()
    {
        // Arrange
        var pool = ArrayPool<byte>.Shared;
        var large = pool.Rent(64);

        // Act
        pool.Return(large);
        var smaller = pool.Rent(8);

        // Assert — the returned array must satisfy the minimum-length contract
        smaller.Length.Should().BeGreaterThanOrEqualTo(8);
    }

    [Fact]
    public void Rent_LargerThanCached_AllocatesNewArray()
    {
        // Arrange
        var pool = ArrayPool<byte>.Shared;
        var small = pool.Rent(4);

        // Act
        pool.Return(small);
        var larger = pool.Rent(256);

        // Assert — cached array is too small, so a fresh one is allocated
        larger.Length.Should().BeGreaterThanOrEqualTo(256);
    }
}
