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
        const string expectedText = "Hello, World!";
        using var innerStream = new MemoryStream();

        // Use a buffered stream to ensure flushing data is required
        using var stream = new BufferedStream(innerStream, bufferSize: 4096);
        using var writer = new StreamWriter(stream);
        await writer.WriteAsync(expectedText);

        // This has to be called before Dispose() because that resets the Encoding
        var expectedBytes = writer.Encoding.GetBytes(expectedText);

        // Act
        await writer.DisposeAsync();

        // Assert
        innerStream.ToArray().Should().Equal(expectedBytes);
    }
}
