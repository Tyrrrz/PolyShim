using System;
using System.Buffers;
using System.IO;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class BinaryReaderTests
{
    [Fact]
    public void Read_ByteSpan_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        using var reader = new BinaryReader(stream);
        var buffer = ArrayPool<byte>.Shared.Rent((int)stream.Length);

        try
        {
            // Act
            var bytesRead = reader.Read(buffer.AsSpan(0, (int)stream.Length));

            // Assert
            bytesRead.Should().Be(5);
            buffer.Should().StartWith([1, 2, 3, 4, 5]);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    [Fact]
    public void Read_CharSpan_Test()
    {
        // Arrange
        using var stream = new MemoryStream("Hello"u8.ToArray());
        using var reader = new BinaryReader(stream);
        var buffer = ArrayPool<char>.Shared.Rent(5);

        try
        {
            // Act
            var charsRead = reader.Read(buffer.AsSpan(0, 5));

            // Assert
            charsRead.Should().Be(5);
            buffer.Should().StartWith("Hello");
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }
}
