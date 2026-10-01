using System;
using System.IO;
using System.IO.Compression;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class ZLibStreamTests
{
    [Fact]
    public void Constructor_NullStream_Test()
    {
        // Act
        var act1 = () => new ZLibStream(null!, CompressionMode.Compress);
        var act2 = () => new ZLibStream(null!, CompressionLevel.Optimal);

        // Assert
        act1.Should().Throw<ArgumentNullException>();
        act2.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Compress_Empty_Test()
    {
        // Arrange
        using var destination = new MemoryStream();

        // Act
        using (
            var zLibStream = new ZLibStream(destination, CompressionMode.Compress, leaveOpen: true)
        )
        {
            // No data written
        }

        // Assert
        destination.ToArray().Should().BeEmpty();
    }

    [Theory]
    [InlineData(CompressionLevel.Optimal, 0x78, 0x9C)]
    [InlineData(CompressionLevel.Fastest, 0x78, 0x01)]
    [InlineData(CompressionLevel.NoCompression, 0x78, 0x01)]
    public void Compress_Header_Test(CompressionLevel level, byte expectedCmf, byte expectedFlg)
    {
        // Arrange
        using var destination = new MemoryStream();

        // Act
        using (var zLibStream = new ZLibStream(destination, level, leaveOpen: true))
            zLibStream.Write(new byte[] { 1, 2, 3 }, 0, 3);

        // Assert
        var result = destination.ToArray();
        result[0].Should().Be(expectedCmf);
        result[1].Should().Be(expectedFlg);
    }

    [Fact]
    public void Compress_Decompress_RoundTrip_Test()
    {
        // Arrange
        var data = new byte[10000];
        new Random(1234567).NextBytes(data);

        using var compressed = new MemoryStream();

        // Act
        using (
            var zLibStream = new ZLibStream(compressed, CompressionLevel.Optimal, leaveOpen: true)
        )
            zLibStream.Write(data, 0, data.Length);

        compressed.Position = 0;

        using var decompressed = new MemoryStream();
        using (
            var zLibStream = new ZLibStream(compressed, CompressionMode.Decompress, leaveOpen: true)
        )
            zLibStream.CopyTo(decompressed);

        // Assert
        decompressed.ToArray().Should().Equal(data);
    }

    [Fact]
    public void Compress_Decompress_SmallWrites_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        using var compressed = new MemoryStream();

        // Act
        using (
            var zLibStream = new ZLibStream(compressed, CompressionLevel.Optimal, leaveOpen: true)
        )
        {
            foreach (var b in data)
                zLibStream.WriteByte(b);
        }

        compressed.Position = 0;

        using var decompressed = new MemoryStream();
        using (
            var zLibStream = new ZLibStream(compressed, CompressionMode.Decompress, leaveOpen: true)
        )
            zLibStream.CopyTo(decompressed);

        // Assert
        decompressed.ToArray().Should().Equal(data);
    }

    [Fact]
    public void Decompress_InteropWithDeflateStream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        using var compressed = new MemoryStream();
        using (
            var deflateStream = new DeflateStream(
                compressed,
                CompressionLevel.Optimal,
                leaveOpen: true
            )
        )
            deflateStream.Write(data, 0, data.Length);

        var rawDeflate = compressed.ToArray();

        // Construct a valid zlib stream manually: header + raw deflate payload + Adler-32 trailer
        using var zlibBytes = new MemoryStream();
        zlibBytes.WriteByte(0x78);
        zlibBytes.WriteByte(0x9C);
        zlibBytes.Write(rawDeflate, 0, rawDeflate.Length);

        var a = 1u;
        var b = 0u;
        foreach (var value in data)
        {
            a = (a + value) % 65521;
            b = (b + a) % 65521;
        }

        var checksum = (b << 16) | a;
        zlibBytes.WriteByte((byte)(checksum >> 24));
        zlibBytes.WriteByte((byte)(checksum >> 16));
        zlibBytes.WriteByte((byte)(checksum >> 8));
        zlibBytes.WriteByte((byte)checksum);

        zlibBytes.Position = 0;

        // Act
        using var decompressed = new MemoryStream();
        using (
            var zLibStream = new ZLibStream(zlibBytes, CompressionMode.Decompress, leaveOpen: true)
        )
            zLibStream.CopyTo(decompressed);

        // Assert
        decompressed.ToArray().Should().Equal(data);
    }

    [Fact]
    public void Decompress_InvalidHeader_Test()
    {
        // Arrange
        using var source = new MemoryStream(new byte[] { 0x00, 0x00, 0x01, 0x02 });
        using var zLibStream = new ZLibStream(source, CompressionMode.Decompress, leaveOpen: true);

        // Act
        var act = () => zLibStream.Read(new byte[10], 0, 10);

        // Assert
        act.Should().Throw<InvalidDataException>();
    }

    [Fact]
    public void CanRead_CanWrite_CanSeek_Test()
    {
        // Arrange
        using var destination = new MemoryStream();

        // Act & assert
        using (
            var compressStream = new ZLibStream(
                destination,
                CompressionMode.Compress,
                leaveOpen: true
            )
        )
        {
            compressStream.CanWrite.Should().BeTrue();
            compressStream.CanRead.Should().BeFalse();
            compressStream.CanSeek.Should().BeFalse();
        }

        destination.Position = 0;

        using (
            var decompressStream = new ZLibStream(
                destination,
                CompressionMode.Decompress,
                leaveOpen: true
            )
        )
        {
            decompressStream.CanRead.Should().BeTrue();
            decompressStream.CanWrite.Should().BeFalse();
        }
    }

    [Fact]
    public void Unsupported_Members_Test()
    {
        // Arrange
        using var destination = new MemoryStream();
        using var zLibStream = new ZLibStream(
            destination,
            CompressionMode.Compress,
            leaveOpen: true
        );

        // Act
        var lengthAct = () =>
        {
            _ = zLibStream.Length;
        };
        var positionGetAct = () =>
        {
            _ = zLibStream.Position;
        };
        var positionSetAct = () => zLibStream.Position = 0;
        var seekAct = () => zLibStream.Seek(0, SeekOrigin.Begin);
        var setLengthAct = () => zLibStream.SetLength(0);

        // Assert
        lengthAct.Should().Throw<NotSupportedException>();
        positionGetAct.Should().Throw<NotSupportedException>();
        positionSetAct.Should().Throw<NotSupportedException>();
        seekAct.Should().Throw<NotSupportedException>();
        setLengthAct.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void LeaveOpen_False_Test()
    {
        // Arrange
        var destination = new MemoryStream();

        // Act
        using (
            var zLibStream = new ZLibStream(destination, CompressionLevel.Optimal, leaveOpen: false)
        )
            zLibStream.Write(new byte[] { 1, 2, 3 }, 0, 3);

        // Assert
        var act = () => destination.WriteByte(0);
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void LeaveOpen_True_Test()
    {
        // Arrange
        using var destination = new MemoryStream();

        // Act
        using (
            var zLibStream = new ZLibStream(destination, CompressionLevel.Optimal, leaveOpen: true)
        )
            zLibStream.Write(new byte[] { 1, 2, 3 }, 0, 3);

        // Assert
        destination.WriteByte(0); // should not throw
    }
}
