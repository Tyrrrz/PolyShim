using System;
using System.Buffers;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class ReadOnlySequenceTests
{
    [Fact]
    public void Slice_Test()
    {
        // Arrange
        var sequence = new ReadOnlySequence<byte>(new byte[] { 10, 20, 30, 40, 50 });

        // Act
        var fromLong = sequence.Slice(1);
        var fromPosition = sequence.Slice(sequence.GetPosition(2, sequence.Start));

        // Assert
        fromLong.ToArray().Should().Equal(20, 30, 40, 50);
        fromPosition.ToArray().Should().Equal(30, 40, 50);
    }

    [Fact]
    public void GetPosition_Test()
    {
        // Arrange
        var sequence = new ReadOnlySequence<byte>(new byte[] { 1, 2, 3, 4, 5 });

        // Act
        var position = sequence.GetPosition(3, sequence.Start);
        var sliced = sequence.Slice(position);

        // Assert
        sliced.ToArray().Should().Equal(4, 5);
    }

    [Fact]
    public void CopyTo_Test()
    {
        // Arrange
        var sequence = new ReadOnlySequence<byte>(new byte[] { 1, 2, 3, 4, 5 });
        var destination = new byte[5];

        // Act
        sequence.CopyTo(destination);

        // Assert
        destination.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void PositionOf_Test()
    {
        // Arrange
        var sequence = new ReadOnlySequence<byte>(new byte[] { 4, 5, 6, 7, 8 });

        // Act
        var position = sequence.PositionOf((byte)7);

        // Assert
        position.Should().NotBeNull();
        sequence.Slice(position!.Value).ToArray().Should().Equal(7, 8);
    }

    [Fact]
    public void ToArray_Test()
    {
        // Arrange
        var sequence = new ReadOnlySequence<byte>(new byte[] { 3, 1, 4, 1, 5 });

        // Act
        var array = sequence.ToArray();

        // Assert
        array.Should().Equal(3, 1, 4, 1, 5);
    }
}
