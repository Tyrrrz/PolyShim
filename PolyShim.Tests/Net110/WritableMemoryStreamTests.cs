using System;
using System.IO;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class WritableMemoryStreamTests
{
    [Fact]
    public void ReadWrite_Test()
    {
        // Arrange
        using var stream = new WritableMemoryStream(new byte[5]);

        // Act
        stream.Write([1, 2, 3], 0, 3);
        stream.Position = 0;

        var buffer = new byte[5];
        var bytesRead = stream.Read(buffer, 0, buffer.Length);

        // Assert
        bytesRead.Should().Be(5);
        buffer.Should().Equal(1, 2, 3, 0, 0);
    }

    [Fact]
    public void WriteBeyondCapacity_Throws_Test()
    {
        // Arrange
        using var stream = new WritableMemoryStream(new byte[3]);

        // Act
        var act = () => stream.Write([1, 2, 3, 4], 0, 4);

        // Assert
        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void SeekPastCapacity_ThenWrite_Throws_Test()
    {
        // Arrange
        using var stream = new WritableMemoryStream(new byte[3]);

        // Act
        stream.Seek(10, SeekOrigin.Begin);
        var act = () => stream.WriteByte(1);

        // Assert
        stream.Position.Should().Be(10);
        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void SetLengthBeyondCapacity_Throws_Test()
    {
        // Arrange
        using var stream = new WritableMemoryStream(new byte[3]);

        // Act
        var act = () => stream.SetLength(4);

        // Assert
        act.Should().Throw<NotSupportedException>();
        stream.Length.Should().Be(3);
    }

    [Fact]
    public void WritePastShrunkenLength_ExtendsAndZeroesGap_Test()
    {
        // Arrange
        using var stream = new WritableMemoryStream(new byte[] { 1, 2, 3, 4, 5, 6 });
        stream.SetLength(2);
        stream.Position = 4;

        // Act
        stream.WriteByte(42);
        stream.Position = 0;

        var readBack = new byte[5];
        var bytesRead = stream.Read(readBack, 0, readBack.Length);

        // Assert
        stream.Length.Should().Be(5);
        bytesRead.Should().Be(5);
        readBack.Should().Equal(1, 2, 0, 0, 42);
    }

    [Fact]
    public void Dispose_Test()
    {
        // Arrange
        var stream = new WritableMemoryStream(new byte[2]);

        // Act
        stream.Dispose();

        // Assert
        stream.CanRead.Should().BeFalse();
        stream.CanSeek.Should().BeFalse();
        stream.CanWrite.Should().BeFalse();

        var act = () => stream.ReadByte();
        act.Should().Throw<ObjectDisposedException>();
    }
}
