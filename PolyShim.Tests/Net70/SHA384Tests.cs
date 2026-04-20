using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class SHA384Tests
{
    [Fact]
    public void HashData_Stream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var hash = SHA384.HashData(stream);

        // Assert
        hash.Should().HaveCount(48);
        hash.Should().Equal(SHA384.HashData(data));
    }

    [Fact]
    public async Task HashDataAsync_Stream_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);

        // Act
        var hash = await SHA384.HashDataAsync(stream);

        // Assert
        hash.Should().HaveCount(48);
        hash.Should().Equal(SHA384.HashData(data));
    }

    [Fact]
    public async Task HashDataAsync_Stream_WithDestination_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);
        var destination = new byte[48];

        // Act
        var bytesWritten = await SHA384.HashDataAsync(stream, destination);

        // Assert
        bytesWritten.Should().Be(48);
        destination.Should().Equal(SHA384.HashData(data));
    }
}
