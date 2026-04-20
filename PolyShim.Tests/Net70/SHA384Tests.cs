using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class SHA384Tests
{
    [Fact]
    public async Task HashDataAsync_Stream_WithDestination_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var destination = new byte[48];

        // Act
        var bytesWritten = await SHA384.HashDataAsync(stream, destination);

        // Assert
        bytesWritten.Should().Be(48);
        // SHA384([1, 2, 3, 4, 5])
        destination.Should()
            .Equal(
                [
                    0xd8, 0x88, 0x75, 0xdb, 0x0f, 0x77, 0xaa, 0xd8,
                    0xf3, 0xd9, 0x94, 0xfe, 0x68, 0xcd, 0x1c, 0xc7,
                    0xec, 0x3a, 0x4f, 0xf1, 0x43, 0x78, 0xb7, 0xfe,
                    0xb9, 0x91, 0xe5, 0x47, 0x84, 0x85, 0x01, 0x92,
                    0x14, 0x58, 0x54, 0xc3, 0x6e, 0x5a, 0x40, 0xa0,
                    0xc2, 0xe8, 0x0d, 0xa2, 0x00, 0x2d, 0x7c, 0xc8,
                ]
            );
    }
}
