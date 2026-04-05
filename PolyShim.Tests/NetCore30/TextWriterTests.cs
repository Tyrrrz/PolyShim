using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class TextWriterTests
{
    [Fact]
    public async Task DisposeAsync_Test()
    {
        // Arrange
        using var innerStream = new MemoryStream();

        // Use a buffered stream to ensure flushing data is required
        using var stream = new BufferedStream(innerStream, bufferSize: 4096);
        using var writer = new StreamWriter(stream);
        await writer.WriteAsync("Hello, World!");

        // Act
        await writer.DisposeAsync();

        // Assert
        innerStream.ToArray().Should().Equal(writer.Encoding.GetBytes("Hello, World!"));
    }
}
