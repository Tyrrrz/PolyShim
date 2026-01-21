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
    public void IsEmpty_Test()
    {
        // Arrange
        ReadOnlySpan<byte> emptySpan = [];
        ReadOnlySpan<byte> nonEmptySpan = [1, 2, 3];

        // Act & Assert
        emptySpan.IsEmpty.Should().BeTrue();
        nonEmptySpan.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void Empty_Test()
    {
        // Act
        var span = ReadOnlySpan<byte>.Empty;

        // Assert
        span.Length.Should().Be(0);
        span.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Slice_WithStartOnly_Test()
    {
        // Arrange
        ReadOnlySpan<byte> span = [1, 2, 3, 4, 5];

        // Act
        var slice = span.Slice(2);

        // Assert
        slice.ToArray().Should().Equal(3, 4, 5);
    }

    [Fact]
    public void ImplicitConversion_FromArraySegment_Test()
    {
        // Arrange
        var array = new byte[] { 1, 2, 3, 4, 5 };
        var segment = new ArraySegment<byte>(array, 1, 3);

        // Act
        ReadOnlySpan<byte> span = segment;

        // Assert
        span.ToArray().Should().Equal(2, 3, 4);
    }

    [Fact]
    public void Length_Test()
    {
        // Arrange
        ReadOnlySpan<byte> span = [1, 2, 3, 4, 5];

        // Act & Assert
        span.Length.Should().Be(5);
    }

    [Fact]
    public void TryCopyTo_Failure_Test()
    {
        // Arrange
        ReadOnlySpan<byte> source = [1, 2, 3, 4, 5];
        Span<byte> destination = stackalloc byte[3];

        // Act
        var result = source.TryCopyTo(destination);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Contains_Test()
    {
        // Arrange
        ReadOnlySpan<int> span = stackalloc int[] { 1, 2, 3, 4, 5 };

        // Act & Assert
        span.Contains(3).Should().BeTrue();
        span.Contains(6).Should().BeFalse();
    }

    [Fact]
    public void IndexOf_Test()
    {
        // Arrange
        ReadOnlySpan<int> span = stackalloc int[] { 1, 2, 3, 4, 5 };

        // Act & Assert
        span.IndexOf(3).Should().Be(2);
        span.IndexOf(1).Should().Be(0);
        span.IndexOf(5).Should().Be(4);
        span.IndexOf(6).Should().Be(-1);
    }

    [Fact]
    public void SequenceEqual_Test()
    {
        // Arrange
        ReadOnlySpan<int> span1 = stackalloc int[] { 1, 2, 3, 4, 5 };
        ReadOnlySpan<int> span2 = stackalloc int[] { 1, 2, 3, 4, 5 };
        ReadOnlySpan<int> span3 = stackalloc int[] { 1, 2, 3, 4, 6 };
        ReadOnlySpan<int> span4 = stackalloc int[] { 1, 2, 3 };

        // Act & Assert
        span1.SequenceEqual(span2).Should().BeTrue();
        span1.SequenceEqual(span3).Should().BeFalse();
        span1.SequenceEqual(span4).Should().BeFalse();
    }
}
