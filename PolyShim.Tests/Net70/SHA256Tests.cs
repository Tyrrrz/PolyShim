using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class SHA256Tests
{
    [Fact]
    public async Task HashDataAsync_Stream_WithDestination_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var destination = new byte[32];

        // Act
        var bytesWritten = await SHA256.HashDataAsync(stream, destination);

        // Assert
        bytesWritten.Should().Be(32);
        // SHA256([1, 2, 3, 4, 5])
        destination.Should()
            .Equal(
                [
                    0x74, 0xf8, 0x1f, 0xe1, 0x67, 0xd9, 0x9b, 0x4c,
                    0xb4, 0x1d, 0x6d, 0x0c, 0xcd, 0xa8, 0x22, 0x78,
                    0xca, 0xee, 0x9f, 0x3e, 0x2f, 0x25, 0xd5, 0xe5,
                    0xa3, 0x93, 0x6f, 0xf3, 0xdc, 0xec, 0x60, 0xd0,
                ]
            );
    }
}
