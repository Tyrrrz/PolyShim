using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class MD5Tests
{
    [Fact]
    public void HashData_Stream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var hash = MD5.HashData(stream);

        // Assert
        hash.Should().HaveCount(16);
        hash.Should().Equal(MD5.HashData(data));
    }

    [Fact]
    public async Task HashDataAsync_Stream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var hash = await MD5.HashDataAsync(stream);

        // Assert
        hash.Should().HaveCount(16);
        hash.Should().Equal(MD5.HashData(data));
    }

    [Fact]
    public async Task HashDataAsync_Stream_WithDestination_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);
        var destination = new byte[16];

        // Act
        var bytesWritten = await MD5.HashDataAsync(stream, destination);

        // Assert
        bytesWritten.Should().Be(16);
        destination.Should().Equal(MD5.HashData(data));
    }
}
