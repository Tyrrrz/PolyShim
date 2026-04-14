using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class MemoryExtensionsTests
{
    [Fact]
    public void Array_AsSpan_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var span = array.AsSpan();

        // Assert
        span.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Array_AsSpan_Start_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var span = array.AsSpan(2);

        // Assert
        span.ToArray().Should().Equal(3, 4, 5);
    }

    [Fact]
    public void Array_AsSpan_Start_Length_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var span = array.AsSpan(1, 3);

        // Assert
        span.ToArray().Should().Equal(2, 3, 4);
    }

    [Fact]
    public void Array_AsMemory_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var memory = array.AsMemory();

        // Assert
        memory.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Array_AsMemory_Start_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var memory = array.AsMemory(2);

        // Assert
        memory.ToArray().Should().Equal(3, 4, 5);
    }

    [Fact]
    public void Array_AsMemory_Start_Length_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var memory = array.AsMemory(1, 3);

        // Assert
        memory.ToArray().Should().Equal(2, 3, 4);
    }

    [Fact]
    public void Array_CopyTo_Span_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3 };
        Span<int> destination = new int[3];

        // Act
        array.CopyTo(destination);

        // Assert
        destination.ToArray().Should().Equal(1, 2, 3);
    }

    [Fact]
    public void Array_CopyTo_Memory_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3 };
        Memory<int> destination = new int[3];

        // Act
        array.CopyTo(destination);

        // Assert
        destination.ToArray().Should().Equal(1, 2, 3);
    }

    [Fact]
    public void ArraySegment_AsSpan_Test()
    {
        // Arrange
        var segment = new ArraySegment<int>([10, 20, 30, 40, 50], 1, 3);

        // Act
        var span = segment.AsSpan();

        // Assert
        span.ToArray().Should().Equal(20, 30, 40);
    }

    [Fact]
    public void String_AsSpan_Test()
    {
        // Act
        var span = "hello".AsSpan();

        // Assert
        span.ToArray().Should().Equal('h', 'e', 'l', 'l', 'o');
    }

    [Fact]
    public void String_AsSpan_Start_Test()
    {
        // Act
        var span = "hello".AsSpan(2);

        // Assert
        span.ToArray().Should().Equal('l', 'l', 'o');
    }

    [Fact]
    public void String_AsSpan_Start_Length_Test()
    {
        // Act
        var span = "hello".AsSpan(1, 3);

        // Assert
        span.ToArray().Should().Equal('e', 'l', 'l');
    }

    [Fact]
    public void String_AsMemory_Test()
    {
        // Act
        var memory = "hello".AsMemory();

        // Assert
        memory.ToArray().Should().Equal('h', 'e', 'l', 'l', 'o');
    }

    [Fact]
    public void String_AsMemory_Start_Test()
    {
        // Act
        var memory = "hello".AsMemory(2);

        // Assert
        memory.ToArray().Should().Equal('l', 'l', 'o');
    }

    [Fact]
    public void String_AsMemory_Start_Length_Test()
    {
        // Act
        var memory = "hello".AsMemory(1, 3);

        // Assert
        memory.ToArray().Should().Equal('e', 'l', 'l');
    }

    [Fact]
    public void Span_IndexOf_Found_Test()
    {
        // Arrange
        Span<int> span = [10, 20, 30, 40, 50];

        // Act & Assert
        span.IndexOf(30).Should().Be(2);
    }

    [Fact]
    public void Span_IndexOf_NotFound_Test()
    {
        // Arrange
        Span<int> span = [10, 20, 30];

        // Act & Assert
        span.IndexOf(99).Should().Be(-1);
    }

    [Fact]
    public void Span_SequenceEqual_Equal_Test()
    {
        // Arrange
        Span<int> span = [1, 2, 3];
        ReadOnlySpan<int> other = [1, 2, 3];

        // Act & Assert
        span.SequenceEqual(other).Should().BeTrue();
    }

    [Fact]
    public void Span_SequenceEqual_NotEqual_Test()
    {
        // Arrange
        Span<int> span = [1, 2, 3];
        ReadOnlySpan<int> other = [1, 2, 4];

        // Act & Assert
        span.SequenceEqual(other).Should().BeFalse();
    }

    [Fact]
    public void Span_SequenceEqual_DifferentLengths_Test()
    {
        // Arrange
        Span<int> span = [1, 2, 3];
        ReadOnlySpan<int> other = [1, 2];

        // Act & Assert
        span.SequenceEqual(other).Should().BeFalse();
    }

    [Fact]
    public void Span_Reverse_Test()
    {
        // Arrange
        Span<int> span = [1, 2, 3, 4, 5];

        // Act
        span.Reverse();

        // Assert
        span.ToArray().Should().Equal(5, 4, 3, 2, 1);
    }

    [Fact]
    public void ReadOnlySpan_IndexOf_Found_Test()
    {
        // Arrange
        ReadOnlySpan<int> span = [10, 20, 30, 40, 50];

        // Act & Assert
        span.IndexOf(30).Should().Be(2);
    }

    [Fact]
    public void ReadOnlySpan_IndexOf_NotFound_Test()
    {
        // Arrange
        ReadOnlySpan<int> span = [10, 20, 30];

        // Act & Assert
        span.IndexOf(99).Should().Be(-1);
    }

    [Fact]
    public void ReadOnlySpan_SequenceEqual_Equal_Test()
    {
        // Arrange
        ReadOnlySpan<int> span = [1, 2, 3];
        ReadOnlySpan<int> other = [1, 2, 3];

        // Act & Assert
        span.SequenceEqual(other).Should().BeTrue();
    }

    [Fact]
    public void ReadOnlySpan_SequenceEqual_NotEqual_Test()
    {
        // Arrange
        ReadOnlySpan<int> span = [1, 2, 3];
        ReadOnlySpan<int> other = [1, 9, 3];

        // Act & Assert
        span.SequenceEqual(other).Should().BeFalse();
    }
}
