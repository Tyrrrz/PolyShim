using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class SHA1Tests
{
    [Fact]
    public void HashData_Stream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var hash = SHA1.HashData(stream);

        // Assert
        hash.Should().HaveCount(20);
        hash.Should().Equal(SHA1.HashData(data));
    }

    [Fact]
    public async Task HashDataAsync_Stream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var hash = await SHA1.HashDataAsync(stream);

        // Assert
        hash.Should().HaveCount(20);
        hash.Should().Equal(SHA1.HashData(data));
    }

    [Fact]
    public async Task HashDataAsync_Stream_WithDestination_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);
        var destination = new byte[20];

        // Act
        var bytesWritten = await SHA1.HashDataAsync(stream, destination);

        // Assert
        bytesWritten.Should().Be(20);
        destination.Should().Equal(SHA1.HashData(data));
    }
}
