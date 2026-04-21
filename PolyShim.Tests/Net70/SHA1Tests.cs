using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class SHA1Tests
{
    [Fact]
    public async Task HashDataAsync_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var destination = new byte[20];

        // Act
        var bytesWritten = await SHA1.HashDataAsync(stream, destination);

        // Assert
        bytesWritten.Should().Be(20);
        destination.Should()
            .Equal(
                [0x11, 0x96, 0x6a, 0xb9, 0xc0, 0x99, 0xf8, 0xfa, 0xbe, 0xfa, 0xc5, 0x4c, 0x08, 0xd5, 0xbe, 0x2b, 0xd8, 0xc9, 0x03, 0xaf]
            );
    }
}
