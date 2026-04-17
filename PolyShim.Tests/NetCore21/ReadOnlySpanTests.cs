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
        var readOnlySpan = (ReadOnlySpan<byte>)[1, 2, 3, 4, 5];

        // Assert
        readOnlySpan.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Indexer_Test()
    {
        // Arrange
        var readOnlySpan = (ReadOnlySpan<byte>)[10, 20, 30, 40, 50];

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
        var readOnlySpan = (ReadOnlySpan<byte>)[10, 20, 30, 40, 50];

        // Act
        var slice = readOnlySpan.Slice(1, 3);

        // Assert
        slice.ToArray().Should().Equal(20, 30, 40);
    }

    [Fact]
    public void CopyTo_Test()
    {
        // Arrange
        var source = (ReadOnlySpan<byte>)[1, 2, 3, 4, 5];
        var destination = (Span<byte>)new byte[5];

        // Act
        source.CopyTo(destination);

        // Assert
        destination.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void TryCopyTo_Test()
    {
        // Arrange
        var source = (ReadOnlySpan<byte>)[1, 2, 3, 4, 5];
        var destination = (Span<byte>)new byte[5];

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
        var readOnlySpan = (ReadOnlySpan<byte>)[1, 2, 3, 4, 5];
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
        var span = (ReadOnlySpan<byte>)[1, 2, 3, 4, 5];
        span.Contains((byte)3).Should().BeTrue();
        span.Contains((byte)10).Should().BeFalse();

        var emptySpan = (ReadOnlySpan<byte>)[];
        emptySpan.Contains((byte)1).Should().BeFalse();
    }
}
