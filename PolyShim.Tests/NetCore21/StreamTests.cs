using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class StreamTests
{
    [Fact]
    public void Read_Array_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = new byte[5];

        // Act
        var bytesRead = stream.Read(buffer);

        // Assert
        bytesRead.Should().Be(5);
        buffer.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Write_Array_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        var buffer = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        stream.Write(buffer);

        // Assert
        stream.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public async Task CopyToAsync_Test()
    {
        // Arrange
        using var source = new MemoryStream([1, 2, 3, 4, 5]);
        using var destination = new MemoryStream();

        // Act
        await source.CopyToAsync(destination);

        // Assert
        destination.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public async Task ReadAsync_Array_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = new byte[stream.Length];

        // Act
        var bytesRead = await stream.ReadAsync(buffer);

        // Assert
        bytesRead.Should().Be(5);
        buffer.Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public async Task WriteAsync_Array_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        var buffer = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        await stream.WriteAsync(buffer);

        // Assert
        stream.ToArray().Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public void Read_Span_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = ArrayPool<byte>.Shared.Rent((int)stream.Length);

        try
        {
            // Act
            var bytesRead = stream.Read(buffer.AsSpan());

            // Assert
            bytesRead.Should().BeGreaterOrEqualTo(5);
            buffer.Should().StartWith(new byte[] { 1, 2, 3, 4, 5 });
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    [Fact]
    public void Write_Span_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        var buffer = ArrayPool<byte>.Shared.Rent(5);

        buffer[0] = 1;
        buffer[1] = 2;
        buffer[2] = 3;
        buffer[3] = 4;
        buffer[4] = 5;

        try
        {
            // Act
            stream.Write(buffer.AsSpan());

            // Assert
            stream.ToArray().Should().StartWith(new byte[] { 1, 2, 3, 4, 5 });
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    [Fact]
    public async Task ReadAsync_Memory_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        using var buffer = MemoryPool<byte>.Shared.Rent((int)stream.Length);

        // Act
        var bytesRead = await stream.ReadAsync(buffer.Memory);

        // Assert
        bytesRead.Should().BeGreaterOrEqualTo(5);
        buffer.Memory.ToArray().Should().StartWith(new byte[] { 1, 2, 3, 4, 5 });
    }

    [Fact]
    public async Task WriteAsync_Memory_Test()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var buffer = MemoryPool<byte>.Shared.Rent(5);

        buffer.Memory.Span[0] = 1;
        buffer.Memory.Span[1] = 2;
        buffer.Memory.Span[2] = 3;
        buffer.Memory.Span[3] = 4;
        buffer.Memory.Span[4] = 5;

        // Act
        await stream.WriteAsync(buffer.Memory);

        // Assert
        stream.ToArray().Should().StartWith(new byte[] { 1, 2, 3, 4, 5 });
    }
}
