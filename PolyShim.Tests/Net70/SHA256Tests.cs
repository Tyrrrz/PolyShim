using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class SHA256Tests
{
    [Fact]
    public void HashData_Stream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var hash = SHA256.HashData(stream);

        // Assert
        hash.Should().HaveCount(32);
        hash.Should().Equal(SHA256.HashData(data));
    }

    [Fact]
    public async Task HashDataAsync_Stream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var hash = await SHA256.HashDataAsync(stream);

        // Assert
        hash.Should().HaveCount(32);
        hash.Should().Equal(SHA256.HashData(data));
    }

    [Fact]
    public async Task HashDataAsync_Stream_WithDestination_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);
        var destination = new byte[32];

        // Act
        var bytesWritten = await SHA256.HashDataAsync(stream, destination);

        // Assert
        bytesWritten.Should().Be(32);
        destination.Should().Equal(SHA256.HashData(data));
    }
}
