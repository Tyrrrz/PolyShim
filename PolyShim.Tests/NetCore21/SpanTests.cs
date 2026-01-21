using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class SpanTests
{
    [Fact]
    public void ImplicitConversion_Test()
    {
        // Act
        Span<byte> span = [1, 2, 3, 4, 5];

        // Assert
        span.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Indexer_Test()
    {
        // Arrange
        Span<byte> span = [10, 20, 30, 40, 50];

        // Act & Assert
        span[0].Should().Be(10);
        span[1].Should().Be(20);
        span[2].Should().Be(30);
        span[3].Should().Be(40);
        span[4].Should().Be(50);
    }

    [Fact]
    public void Slice_Test()
    {
        // Arrange
        Span<byte> span = [1, 2, 3, 4, 5];

        // Act
        var slice = span.Slice(1, 3);

        // Assert
        slice.ToArray().Should().Equal(2, 3, 4);
    }

    [Fact]
    public void Fill_Test()
    {
        // Arrange
        Span<byte> span = [1, 2, 3, 4, 5];

        // Act
        span.Fill(42);

        // Assert
        span.ToArray().Should().Equal(42, 42, 42, 42, 42);
    }

    [Fact]
    public void Clear_Test()
    {
        // Arrange
        Span<byte> span = [1, 2, 3, 4, 5];

        // Act
        span.Clear();

        // Assert
        span.ToArray().Should().Equal(0, 0, 0, 0, 0);
    }

    [Fact]
    public void CopyTo_Test()
    {
        // Arrange
        Span<byte> source = [1, 2, 3, 4, 5];
        Span<byte> destination = stackalloc byte[5];

        // Act
        source.CopyTo(destination);

        // Assert
        destination.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void TryCopyTo_Test()
    {
        // Arrange
        Span<byte> source = [1, 2, 3, 4, 5];
        Span<byte> destination = stackalloc byte[5];

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
        Span<byte> span = [1, 2, 3, 4, 5];
        var collected = new List<byte>();

        // Act
        foreach (var item in span)
            collected.Add(item);

        // Assert
        collected.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void IsEmpty_Test()
    {
        // Arrange
        Span<byte> emptySpan = [];
        Span<byte> nonEmptySpan = [1, 2, 3];

        // Act & Assert
        emptySpan.IsEmpty.Should().BeTrue();
        nonEmptySpan.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void Empty_Test()
    {
        // Act
        var span = Span<byte>.Empty;

        // Assert
        span.Length.Should().Be(0);
        span.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Slice_WithStartOnly_Test()
    {
        // Arrange
        Span<byte> span = [1, 2, 3, 4, 5];

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
        Span<byte> span = segment;

        // Assert
        span.ToArray().Should().Equal(2, 3, 4);
    }

    [Fact]
    public void ImplicitConversion_ToReadOnlySpan_Test()
    {
        // Arrange
        Span<byte> span = [1, 2, 3, 4, 5];

        // Act
        ReadOnlySpan<byte> readOnlySpan = span;

        // Assert
        readOnlySpan.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Length_Test()
    {
        // Arrange
        Span<byte> span = [1, 2, 3, 4, 5];

        // Act & Assert
        span.Length.Should().Be(5);
    }

    [Fact]
    public void TryCopyTo_Failure_Test()
    {
        // Arrange
        Span<byte> source = [1, 2, 3, 4, 5];
        Span<byte> destination = stackalloc byte[3];

        // Act
        var result = source.TryCopyTo(destination);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Array_AsSpan_WithStartAndLength_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var span = array.AsSpan(1, 3);

        // Assert
        span.ToArray().Should().Equal(2, 3, 4);
    }

    [Fact]
    public void Array_AsSpan_WithStart_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var span = array.AsSpan(2);

        // Assert
        span.ToArray().Should().Equal(3, 4, 5);
    }

    [Fact]
    public void Array_AsSpan_Default_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var span = array.AsSpan();

        // Assert
        span.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Array_CopyTo_Span_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };
        Span<int> destination = stackalloc int[5];

        // Act
        array.CopyTo(destination);

        // Assert
        destination.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Contains_Test()
    {
        // Arrange
        Span<int> span = stackalloc int[] { 1, 2, 3, 4, 5 };

        // Act & Assert
        span.Contains(3).Should().BeTrue();
        span.Contains(6).Should().BeFalse();
    }

    [Fact]
    public void IndexOf_Test()
    {
        // Arrange
        Span<int> span = stackalloc int[] { 1, 2, 3, 4, 5 };

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
        Span<int> span1 = stackalloc int[] { 1, 2, 3, 4, 5 };
        ReadOnlySpan<int> span2 = stackalloc int[] { 1, 2, 3, 4, 5 };
        ReadOnlySpan<int> span3 = stackalloc int[] { 1, 2, 3, 4, 6 };
        ReadOnlySpan<int> span4 = stackalloc int[] { 1, 2, 3 };

        // Act & Assert
        span1.SequenceEqual(span2).Should().BeTrue();
        span1.SequenceEqual(span3).Should().BeFalse();
        span1.SequenceEqual(span4).Should().BeFalse();
    }

    [Fact]
    public void Reverse_Test()
    {
        // Arrange
        Span<int> span = stackalloc int[] { 1, 2, 3, 4, 5 };

        // Act
        span.Reverse();

        // Assert
        span.ToArray().Should().Equal(5, 4, 3, 2, 1);
    }
}
