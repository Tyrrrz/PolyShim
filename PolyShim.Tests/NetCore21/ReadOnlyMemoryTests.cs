using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class ReadOnlyMemoryTests
{
    [Fact]
    public void ImplicitConversion_Test()
    {
        // Act
        ReadOnlyMemory<byte> memory = new byte[] { 1, 2, 3, 4, 5 };

        // Assert
        memory.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Slice_Test()
    {
        // Arrange
        ReadOnlyMemory<byte> memory = new byte[] { 10, 20, 30, 40, 50 };

        // Act & Assert
        memory.Slice(0, 2).ToArray().Should().Equal(10, 20);
        memory.Slice(2, 2).ToArray().Should().Equal(30, 40);
        memory.Slice(4, 1).ToArray().Should().Equal(50);
    }

    [Fact]
    public void CopyTo_Test()
    {
        // Arrange
        ReadOnlyMemory<byte> source = new byte[] { 1, 2, 3, 4, 5 };
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
        ReadOnlyMemory<byte> source = new byte[] { 1, 2, 3, 4, 5 };
        Memory<byte> destination = new byte[5];

        // Act
        var result = source.TryCopyTo(destination);

        // Assert
        result.Should().BeTrue();
        destination.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }
}
