using System;
using System.IO;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class ReadOnlyMemoryStreamTests
{
    [Fact]
    public void Read_Test()
    {
        // Arrange
        byte[] source = [1, 2, 3, 4, 5];
        using var stream = new ReadOnlyMemoryStream(source);
        var destination = new byte[source.Length];

        // Act
        var bytesRead = stream.Read(destination, 0, destination.Length);

        // Assert
        bytesRead.Should().Be(5);
        destination.Should().Equal(source);
        stream.Position.Should().Be(5);
    }

    [Fact]
    public void Read_Empty_Test()
    {
        // Arrange
        using var stream = new ReadOnlyMemoryStream(ReadOnlyMemory<byte>.Empty);
        var result = new byte[8];

        // Act
        var bytesRead = stream.Read(result, 0, result.Length);

        // Assert
        bytesRead.Should().Be(0);
        stream.ReadByte().Should().Be(-1);
    }

    [Fact]
    public void Seek_Test()
    {
        // Arrange
        byte[] buffer = [10, 20, 30, 40, 50];
        using var stream = new ReadOnlyMemoryStream(buffer);

        // Act
        var position = stream.Seek(2, SeekOrigin.Begin);
        var value = stream.ReadByte();

        // Assert
        position.Should().Be(2);
        value.Should().Be(30);
        stream.Position.Should().Be(3);
    }

    [Fact]
    public void Write_NotSupported_Test()
    {
        // Arrange
        using var stream = new ReadOnlyMemoryStream(new byte[] { 1, 2, 3 });

        // Act
        var writeAct = () => stream.Write(new byte[1], 0, 1);
        var setLengthAct = () => stream.SetLength(1);

        // Assert
        writeAct.Should().Throw<NotSupportedException>();
        setLengthAct.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void Dispose_Test()
    {
        // Arrange
        var stream = new ReadOnlyMemoryStream(new byte[] { 1, 2, 3 });

        // Act
        stream.Dispose();

        // Assert
        stream.CanRead.Should().BeFalse();
        stream.CanSeek.Should().BeFalse();

        var readAct = () => stream.ReadByte();
        readAct.Should().Throw<ObjectDisposedException>();
    }
}
