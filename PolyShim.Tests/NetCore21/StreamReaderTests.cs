using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class StreamReaderTests
{
    [Fact]
    public void Read_Array_Test()
    {
        // Arrange
        using var stream = new MemoryStream("Hello world"u8.ToArray());
        using var reader = new StreamReader(stream);
        var buffer = new char[stream.Length];

        // Act
        var charsRead = reader.Read(buffer);

        // Assert
        charsRead.Should().Be(11);
        buffer.Should().StartWith("Hello world");
    }

    [Fact]
    public async Task ReadAsync_Array_Test()
    {
        // Arrange
        using var stream = new MemoryStream("Hello world"u8.ToArray());
        using var reader = new StreamReader(stream);
        var buffer = new char[stream.Length];

        // Act
        var charsRead = await reader.ReadAsync(buffer);

        // Assert
        charsRead.Should().Be(11);
        buffer.Should().StartWith("Hello world");
    }

    [Fact]
    public void Read_Span_Test()
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
    public async Task ReadAsync_Memory_Test()
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