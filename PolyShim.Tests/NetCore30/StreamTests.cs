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
        var innerStream = new MemoryStream();

        // Use a buffered stream to ensure DisposeAsync flushes data
        var stream = new BufferedStream(innerStream, bufferSize: 4096);
        await stream.WriteAsync(new byte[] { 1, 2, 3, 4, 5 });

        // Act
        await stream.DisposeAsync();

        // Assert
        innerStream.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }
}
