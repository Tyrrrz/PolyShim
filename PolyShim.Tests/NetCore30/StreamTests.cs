using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class StreamTests
{
    [Fact]
    public async Task DisposeAsync_Test()
    {
        // Arrange
        var stream = new MemoryStream();

        // Act
        await stream.DisposeAsync();

        // Assert
        stream.CanRead.Should().BeFalse();
    }

    [Fact]
    public async Task DisposeAsync_FlushesData_Test()
    {
        // Arrange
        var innerStream = new MemoryStream();
        var stream = new BufferedStream(innerStream, bufferSize: 4096);
        stream.Write([1, 2, 3, 4, 5], 0, 5);

        // Act
        await stream.DisposeAsync();

        // Assert (data should have been flushed to the inner stream)
        innerStream.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }
}
