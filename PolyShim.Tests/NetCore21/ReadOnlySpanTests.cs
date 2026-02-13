using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class ReadOnlySpanTests
{
    [Fact]
    public void ImplicitConversion_Test()
    {
        // Act
        ReadOnlySpan<byte> readOnlySpan = [1, 2, 3, 4, 5];

        // Assert
        readOnlySpan.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Indexer_Test()
    {
        // Arrange
        ReadOnlySpan<byte> readOnlySpan = [10, 20, 30, 40, 50];

        // Act & Assert
        readOnlySpan[0].Should().Be(10);
        readOnlySpan[1].Should().Be(20);
        readOnlySpan[2].Should().Be(30);
        readOnlySpan[3].Should().Be(40);
        readOnlySpan[4].Should().Be(50);
    }

    [Fact]
    public void Slice_Test()
    {
        // Arrange
        ReadOnlySpan<byte> readOnlySpan = [10, 20, 30, 40, 50];

        // Act
        var slice = readOnlySpan.Slice(1, 3);

        // Assert
        slice.ToArray().Should().Equal(20, 30, 40);
    }

    [Fact]
    public void CopyTo_Test()
    {
        // Arrange
        ReadOnlySpan<byte> source = [1, 2, 3, 4, 5];
        Span<byte> destination = new byte[5];

        // Act
        source.CopyTo(destination);

        // Assert
        destination.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void TryCopyTo_Test()
    {
        // Arrange
        ReadOnlySpan<byte> source = [1, 2, 3, 4, 5];
        Span<byte> destination = new byte[5];

        // Act
        var result = source.TryCopyTo(destination);

        // Assert
        result.Should().BeTrue();
        destination.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Enumeration_Test()
    {
        // Arrange
        ReadOnlySpan<byte> readOnlySpan = [1, 2, 3, 4, 5];
        var result = new List<byte>();

        // Act
        foreach (var item in readOnlySpan)
            result.Add(item);

        // Assert
        result.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Contains_Test()
    {
        // Act & Assert
        ReadOnlySpan<byte> span = [1, 2, 3, 4, 5];
        span.Contains((byte)3).Should().BeTrue();
        span.Contains((byte)10).Should().BeFalse();

        ReadOnlySpan<byte> emptySpan = [];
        emptySpan.Contains((byte)1).Should().BeFalse();
    }
}
