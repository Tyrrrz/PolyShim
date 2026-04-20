using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class MD5Tests
{
    // MD5([1, 2, 3, 4, 5])
    private static readonly byte[] ExpectedHash =
    [
        0x7c,
        0xfd,
        0xd0,
        0x78,
        0x89,
        0xb3,
        0x29,
        0x5d,
        0x6a,
        0x55,
        0x09,
        0x14,
        0xab,
        0x35,
        0xe0,
        0x68,
    ];

    [Fact]
    public void HashData_Stream_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);

        // Act
        var hash = MD5.HashData(stream);

        // Assert
        hash.Should().Equal(ExpectedHash);
    }

    [Fact]
    public async Task HashDataAsync_Stream_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);

        // Act
        var hash = await MD5.HashDataAsync(stream);

        // Assert
        hash.Should().Equal(ExpectedHash);
    }

    [Fact]
    public async Task HashDataAsync_Stream_WithDestination_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var destination = new byte[16];

        // Act
        var bytesWritten = await MD5.HashDataAsync(stream, destination);

        // Assert
        bytesWritten.Should().Be(16);
        destination.Should().Equal(ExpectedHash);
    }
}
