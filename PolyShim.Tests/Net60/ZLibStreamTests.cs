using System;
using System.IO;
using System.IO.Compression;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class ZLibStreamTests
{
    [Fact]
    public void Compress_Decompress_RoundTrip_Test()
    {
        // Arrange
        var data = new byte[10000];
        new Random(1234567).NextBytes(data);

        using var compressed = new MemoryStream();

        // Act
        using (var compression = new ZLibStream(compressed, CompressionLevel.Optimal, true))
            compression.Write(data, 0, data.Length);

        compressed.Position = 0;

        using var decompressed = new MemoryStream();
        using (var decompression = new ZLibStream(compressed, CompressionMode.Decompress, true))
            decompression.CopyTo(decompressed);

        // Assert
        decompressed.ToArray().Should().Equal(data);
    }

    [Fact]
    public void Compress_Empty_Test()
    {
        // Arrange
        using var destination = new MemoryStream();

        // Act
        using (new ZLibStream(destination, CompressionMode.Compress, true))
        {
            // No data written
        }

        // Assert
        destination.ToArray().Should().BeEmpty();
    }

    [Fact]
    public void Dispose_Test()
    {
        // Arrange
        var destination = new MemoryStream();

        // Act
        using (var compression = new ZLibStream(destination, CompressionLevel.Optimal))
            compression.Write([1, 2, 3], 0, 3);

        // Assert
        var act = () => destination.WriteByte(0);
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void Dispose_LeaveOpen_Test()
    {
        // Arrange
        using var destination = new MemoryStream();

        // Act
        using (var compression = new ZLibStream(destination, CompressionLevel.Optimal, true))
            compression.Write([1, 2, 3], 0, 3);

        // Assert
        destination.WriteByte(0); // should not throw
    }
}
