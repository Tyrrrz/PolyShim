using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class SHA512Tests
{
    // SHA512([1, 2, 3, 4, 5])
    private static readonly byte[] ExpectedHash =
    [
        0x50,
        0x54,
        0x0b,
        0xc4,
        0xae,
        0x31,
        0x87,
        0x5f,
        0xce,
        0xb3,
        0x82,
        0x94,
        0x34,
        0xc5,
        0x5e,
        0x3c,
        0x2b,
        0x66,
        0xdd,
        0xd7,
        0x22,
        0x7a,
        0x88,
        0x3a,
        0x3b,
        0x4c,
        0xc8,
        0xf6,
        0xcd,
        0xa9,
        0x65,
        0xad,
        0x17,
        0x12,
        0xb3,
        0xee,
        0x00,
        0x08,
        0xf9,
        0xce,
        0xe0,
        0x8d,
        0xa9,
        0x3f,
        0x52,
        0x34,
        0xc1,
        0xa7,
        0xbf,
        0x0e,
        0x25,
        0x70,
        0xef,
        0x56,
        0xd6,
        0x52,
        0x80,
        0xff,
        0xea,
        0x69,
        0x1b,
        0x95,
        0x3e,
        0xfe,
    ];

    [Fact]
    public void HashData_Stream_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);

        // Act
        var hash = SHA512.HashData(stream);

        // Assert
        hash.Should().Equal(ExpectedHash);
    }

    [Fact]
    public async Task HashDataAsync_Stream_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);

        // Act
        var hash = await SHA512.HashDataAsync(stream);

        // Assert
        hash.Should().Equal(ExpectedHash);
    }

    [Fact]
    public async Task HashDataAsync_Stream_WithDestination_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var destination = new byte[64];

        // Act
        var bytesWritten = await SHA512.HashDataAsync(stream, destination);

        // Assert
        bytesWritten.Should().Be(64);
        destination.Should().Equal(ExpectedHash);
    }
}
