using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class MemoryTests
{
    [Fact]
    public void ImplicitConversion_Test()
    {
        // Act
        Memory<byte> memory = new byte[] { 1, 2, 3, 4, 5 };

        // Assert
        memory.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Slice_Test()
    {
        // Arrange
        Memory<byte> memory = new byte[] { 10, 20, 30, 40, 50 };

        // Act & Assert
        memory.Slice(0, 2).ToArray().Should().Equal(10, 20);
        memory.Slice(2, 2).ToArray().Should().Equal(30, 40);
        memory.Slice(4, 1).ToArray().Should().Equal(50);
    }

    [Fact]
    public void CopyTo_Test()
    {
        // Arrange
        Memory<byte> source = new byte[] { 1, 2, 3, 4, 5 };
        Memory<byte> destination = new byte[5];

        // Act
        source.CopyTo(destination);

        // Assert
        destination.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void TryCopyTo_Test()
    {
        // Arrange
        Memory<byte> source = new byte[] { 1, 2, 3, 4, 5 };
        Memory<byte> destination = new byte[5];

        // Act
        var result = source.TryCopyTo(destination);

        // Assert
        result.Should().BeTrue();
        destination.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void IsEmpty_Test()
    {
        // Arrange
        Memory<byte> emptyMemory = Memory<byte>.Empty;
        Memory<byte> nonEmptyMemory = new byte[] { 1, 2, 3 };

        // Act & Assert
        emptyMemory.IsEmpty.Should().BeTrue();
        nonEmptyMemory.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void Empty_Test()
    {
        // Act
        var memory = Memory<byte>.Empty;

        // Assert
        memory.Length.Should().Be(0);
        memory.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void Slice_WithStartOnly_Test()
    {
        // Arrange
        Memory<byte> memory = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var slice = memory.Slice(2);

        // Assert
        slice.ToArray().Should().Equal(3, 4, 5);
    }

    [Fact]
    public void Span_Property_Test()
    {
        // Arrange
        Memory<byte> memory = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var span = memory.Span;

        // Assert
        span.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Equals_Test()
    {
        // Arrange
        var array = new byte[] { 1, 2, 3, 4, 5 };
        Memory<byte> memory1 = new Memory<byte>(array, 0, 5);
        Memory<byte> memory2 = new Memory<byte>(array, 0, 5);
        Memory<byte> memory3 = new Memory<byte>(array, 1, 4);
        Memory<byte> memory4 = new byte[] { 1, 2, 3, 4, 5 };

        // Act & Assert
        memory1.Equals(memory2).Should().BeTrue();
        memory1.Equals(memory3).Should().BeFalse();
        memory1.Equals(memory4).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_Test()
    {
        // Arrange
        var array = new byte[] { 1, 2, 3, 4, 5 };
        Memory<byte> memory1 = new Memory<byte>(array, 0, 5);
        Memory<byte> memory2 = new Memory<byte>(array, 0, 5);

        // Act & Assert
        memory1.GetHashCode().Should().Be(memory2.GetHashCode());
    }

    [Fact]
    public void ImplicitConversion_FromArraySegment_Test()
    {
        // Arrange
        var array = new byte[] { 1, 2, 3, 4, 5 };
        var segment = new ArraySegment<byte>(array, 1, 3);

        // Act
        Memory<byte> memory = segment;

        // Assert
        memory.ToArray().Should().Equal(2, 3, 4);
    }

    [Fact]
    public void ImplicitConversion_ToReadOnlyMemory_Test()
    {
        // Arrange
        Memory<byte> memory = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        ReadOnlyMemory<byte> readOnlyMemory = memory;

        // Assert
        readOnlyMemory.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Length_Test()
    {
        // Arrange
        Memory<byte> memory = new byte[] { 1, 2, 3, 4, 5 };

        // Act & Assert
        memory.Length.Should().Be(5);
    }

    [Fact]
    public void TryCopyTo_Failure_Test()
    {
        // Arrange
        Memory<byte> source = new byte[] { 1, 2, 3, 4, 5 };
        Memory<byte> destination = new byte[3];

        // Act
        var result = source.TryCopyTo(destination);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Array_AsMemory_WithStartAndLength_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var memory = array.AsMemory(1, 3);

        // Assert
        memory.ToArray().Should().Equal(2, 3, 4);
    }

    [Fact]
    public void Array_AsMemory_WithStart_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var memory = array.AsMemory(2);

        // Assert
        memory.ToArray().Should().Equal(3, 4, 5);
    }

    [Fact]
    public void Array_AsMemory_Default_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var memory = array.AsMemory();

        // Assert
        memory.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Array_CopyTo_Memory_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };
        Memory<int> destination = new int[5];

        // Act
        array.CopyTo(destination);

        // Assert
        destination.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }
}
