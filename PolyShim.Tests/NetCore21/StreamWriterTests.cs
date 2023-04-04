using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class StreamWriterTests
{
    [Fact]
    public void Write_Span_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        var buffer = ArrayPool<char>.Shared.Rent(11);

        buffer[0] = 'H';
        buffer[1] = 'e';
        buffer[2] = 'l';
        buffer[3] = 'l';
        buffer[4] = 'o';
        buffer[5] = ' ';
        buffer[6] = 'w';
        buffer[7] = 'o';
        buffer[8] = 'r';
        buffer[9] = 'l';
        buffer[10] = 'd';

        try
        {
            // Act
            writer.Write(buffer.AsSpan());
            writer.Flush();

            // Assert
            stream.ToArray().Should().StartWith("Hello world"u8.ToArray());
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }

    [Fact]
    public async Task WriteAsync_Memory_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        using var buffer = MemoryPool<char>.Shared.Rent(11);

        buffer.Memory.Span[0] = 'H';
        buffer.Memory.Span[1] = 'e';
        buffer.Memory.Span[2] = 'l';
        buffer.Memory.Span[3] = 'l';
        buffer.Memory.Span[4] = 'o';
        buffer.Memory.Span[5] = ' ';
        buffer.Memory.Span[6] = 'w';
        buffer.Memory.Span[7] = 'o';
        buffer.Memory.Span[8] = 'r';
        buffer.Memory.Span[9] = 'l';
        buffer.Memory.Span[10] = 'd';

        // Act
        await writer.WriteAsync(buffer.Memory);
        await writer.FlushAsync();

        // Assert
        stream.ToArray().Should().StartWith("Hello world"u8.ToArray());
    }
}