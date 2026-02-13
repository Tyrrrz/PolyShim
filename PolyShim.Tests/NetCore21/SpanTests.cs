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
    public void Contains_WithExistingValue_ReturnsTrue()
    {
        // Arrange
        Span<byte> span = [1, 2, 3, 4, 5];

        // Act & Assert
        span.Contains((byte)3).Should().BeTrue();
    }

    [Fact]
    public void Contains_WithNonExistingValue_ReturnsFalse()
    {
        // Arrange
        Span<byte> span = [1, 2, 3, 4, 5];

        // Act & Assert
        span.Contains((byte)10).Should().BeFalse();
    }

    [Fact]
    public void Contains_WithEmptySpan_ReturnsFalse()
    {
        // Arrange
        Span<byte> span = [];

        // Act & Assert
        span.Contains((byte)1).Should().BeFalse();
    }

    [Fact]
    public void Contains_WithReferenceType_ReturnsTrue()
    {
        // Arrange
        var target = "hello";
        Span<string> span = [target, "world", "test"];

        // Act & Assert
        span.Contains(target).Should().BeTrue();
    }

    [Fact]
    public void Contains_WithReferenceType_ReturnsFalse()
    {
        // Arrange
        Span<string> span = ["hello", "world", "test"];

        // Act & Assert
        span.Contains("missing").Should().BeFalse();
    }
}
