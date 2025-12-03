using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class TextReaderTests
{
    [Fact]
    public void Read_Test()
    {
        // Arrange
        using var stream = new MemoryStream("Hello world"u8.ToArray());
        using var reader = new StreamReader(stream);
        var buffer = ArrayPool<char>.Shared.Rent((int)stream.Length);

        try
        {
            // Act
            var charsRead = reader.Read(buffer.AsSpan());

            // Assert
            charsRead.Should().Be(11);
            buffer.Should().StartWith("Hello world");
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }

    [Fact]
    public async Task ReadAsync_Test()
    {
        // Arrange
        using var stream = new MemoryStream("Hello world"u8.ToArray());
        using var reader = new StreamReader(stream);
        using var buffer = MemoryPool<char>.Shared.Rent((int)stream.Length);

        // Act
        var charsRead = await reader.ReadAsync(buffer.Memory);

        // Assert
        charsRead.Should().Be(11);
        buffer.Memory.ToArray().Should().StartWith("Hello world");
    }
}
