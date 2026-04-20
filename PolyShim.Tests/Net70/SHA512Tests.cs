using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class SHA512Tests
{
    [Fact]
    public void HashData_Stream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var hash = SHA512.HashData(stream);

        // Assert
        hash.Should().HaveCount(64);
        hash.Should().Equal(SHA512.HashData(data));
    }

    [Fact]
    public async Task HashDataAsync_Stream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var hash = await SHA512.HashDataAsync(stream);

        // Assert
        hash.Should().HaveCount(64);
        hash.Should().Equal(SHA512.HashData(data));
    }

    [Fact]
    public async Task HashDataAsync_Stream_WithDestination_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);
        var destination = new byte[64];

        // Act
        var bytesWritten = await SHA512.HashDataAsync(stream, destination);

        // Assert
        bytesWritten.Should().Be(64);
        destination.Should().Equal(SHA512.HashData(data));
    }
}
