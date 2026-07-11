using System;
using System.Buffers;
using System.IO;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class BinaryWriterTests
{
    [Fact]
    public void Write_ByteSpan_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        var buffer = ArrayPool<byte>.Shared.Rent(5);

        buffer[0] = 1;
        buffer[1] = 2;
        buffer[2] = 3;
        buffer[3] = 4;
        buffer[4] = 5;

        try
        {
            // Act
            writer.Write(buffer.AsSpan(0, 5));

            // Assert
            stream.ToArray().Should().Equal([1, 2, 3, 4, 5]);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    [Fact]
    public void Write_CharSpan_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        var buffer = ArrayPool<char>.Shared.Rent(5);

        buffer[0] = 'H';
        buffer[1] = 'e';
        buffer[2] = 'l';
        buffer[3] = 'l';
        buffer[4] = 'o';

        try
        {
            // Act
            writer.Write(buffer.AsSpan(0, 5));

            // Assert
            stream.ToArray().Should().Equal("Hello"u8.ToArray());
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }
}
