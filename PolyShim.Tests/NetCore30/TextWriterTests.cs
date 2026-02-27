using System.IO;
using System.Text;
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
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);

        // Act & assert (should not throw)
        await writer.DisposeAsync();
    }

    [Fact]
    public async Task DisposeAsync_FlushesData_Test()
    {
        // Arrange
        var stream = new MemoryStream();
        var writer = new StreamWriter(
            stream,
            new UTF8Encoding(false),
            bufferSize: 4096,
            leaveOpen: true
        );
        writer.Write("Hello world");

        // Act
        await writer.DisposeAsync();

        // Assert (data should have been flushed to the underlying stream)
        stream.ToArray().Should().StartWith("Hello world"u8.ToArray());
    }
}
