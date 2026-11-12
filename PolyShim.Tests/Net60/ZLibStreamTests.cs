using System;
using System.IO;
using System.IO.Compression;
using AwesomeAssertions;
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
        // The exact bytes differ between implementations: .NET 10 and earlier (and the polyfill)
        // write nothing, while .NET 11+ writes a complete, valid zlib stream for empty input.
        // Both are acceptable as long as the result decompresses to empty data.
        destination.Position = 0;

        using var decompressed = new MemoryStream();
        using (var decompression = new ZLibStream(destination, CompressionMode.Decompress, true))
            decompression.CopyTo(decompressed);

        decompressed.ToArray().Should().BeEmpty();
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
