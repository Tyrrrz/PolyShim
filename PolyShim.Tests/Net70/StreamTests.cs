using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class StreamTests
{
    [Fact]
    public void ReadAtLeast_Array_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = new byte[3];

        // Act
        var bytesRead = stream.ReadAtLeast(buffer, 3);

        // Assert
        bytesRead.Should().Be(3);
        buffer.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void ReadAtLeast_Array_Overflow_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = new byte[10];

        // Act & assert
        Assert.ThrowsAny<EndOfStreamException>(() => stream.ReadAtLeast(buffer, 10));
    }

    [Fact]
    public void ReadAtLeast_Array_SilentOverflow_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = new byte[10];

        // Act
        var bytesRead = stream.ReadAtLeast(buffer, 10, false);

        // Assert
        bytesRead.Should().Be(5);
        buffer.Should().StartWith(new byte[] { 1, 2, 3, 4, 5 });
    }

    [Fact]
    public void ReadExactly_Array_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = new byte[3];

        // Act
        stream.ReadExactly(buffer);

        // Assert
        buffer.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void ReadExactly_Array_OffsetAndCount_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = new byte[3];

        // Act
        stream.ReadExactly(buffer, 1, 1);

        // Assert
        buffer.Should().Equal(0, 1, 0);
    }

    [Fact]
    public async Task ReadAtLeastAsync_Array_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = new byte[3];

        // Act
        var bytesRead = await stream.ReadAtLeastAsync(buffer, 3);

        // Assert
        bytesRead.Should().Be(3);
        buffer.Should().Equal(1, 2, 3);
    }

    [Fact]
    public async Task ReadAtLeastAsync_Array_Overflow_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = new byte[10];

        // Act & assert
        await Assert.ThrowsAnyAsync<EndOfStreamException>(async () =>
            await stream.ReadAtLeastAsync(buffer, 10)
        );
    }

    [Fact]
    public async Task ReadAtLeastAsync_Array_SilentOverflow_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = new byte[10];

        // Act
        var bytesRead = await stream.ReadAtLeastAsync(buffer, 10, false);

        // Assert
        bytesRead.Should().Be(5);
        buffer.Should().StartWith(new byte[] { 1, 2, 3, 4, 5 });
    }

    [Fact]
    public async Task ReadExactlyAsync_Array_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = new byte[3];

        // Act
        await stream.ReadExactlyAsync(buffer);

        // Assert
        buffer.Should().Equal(1, 2, 3);
    }

    [Fact]
    public async Task ReadExactlyAsync_Array_OffsetAndCount_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = new byte[3];

        // Act
        await stream.ReadExactlyAsync(buffer, 1, 1);

        // Assert
        buffer.Should().Equal(0, 1, 0);
    }

    [Fact]
    public void ReadAtLeast_Span_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = ArrayPool<byte>.Shared.Rent(3);

        try
        {
            // Act
            var bytesRead = stream.ReadAtLeast(buffer.AsSpan(), 3);

            // Assert
            bytesRead.Should().BeGreaterOrEqualTo(3);
            buffer.Should().StartWith(new byte[] { 1, 2, 3 });
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    [Fact]
    public void ReadAtLeast_Span_Overflow_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = ArrayPool<byte>.Shared.Rent(10);

        // Act & assert
        try
        {
            Assert.Throws<EndOfStreamException>(() => stream.ReadAtLeast(buffer.AsSpan(), 10));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    [Fact]
    public void ReadAtLeast_Span_SilentOverflow_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = ArrayPool<byte>.Shared.Rent(10);

        try
        {
            // Act
            var bytesRead = stream.ReadAtLeast(buffer.AsSpan(), 10, false);

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
    public void ReadExactly_Span_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        var buffer = ArrayPool<byte>.Shared.Rent(3);

        try
        {
            // Act
            stream.ReadExactly(buffer.AsSpan()[..3]);

            // Assert
            buffer.Should().StartWith(new byte[] { 1, 2, 3 });
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    [Fact]
    public async Task ReadAtLeastAsync_Memory_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        using var buffer = MemoryPool<byte>.Shared.Rent(3);

        // Act
        var bytesRead = await stream.ReadAtLeastAsync(buffer.Memory, 3);

        // Assert
        bytesRead.Should().BeGreaterOrEqualTo(3);
        buffer.Memory.ToArray().Should().StartWith(new byte[] { 1, 2, 3 });
    }

    [Fact]
    public async Task ReadAtLeastAsync_Memory_Overflow_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        using var buffer = MemoryPool<byte>.Shared.Rent(10);

        // Act & assert
        await Assert.ThrowsAnyAsync<EndOfStreamException>(async () =>
            await stream.ReadAtLeastAsync(buffer.Memory, 10)
        );
    }

    [Fact]
    public async Task ReadAtLeastAsync_Memory_SilentOverflow_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        using var buffer = MemoryPool<byte>.Shared.Rent(10);

        // Act
        var bytesRead = await stream.ReadAtLeastAsync(buffer.Memory, 10, false);

        // Assert
        bytesRead.Should().BeGreaterOrEqualTo(5);
        buffer.Memory.ToArray().Should().StartWith(new byte[] { 1, 2, 3, 4 });
    }

    [Fact]
    public async Task ReadExactlyAsync_Memory_Test()
    {
        // Arrange
        using var stream = new MemoryStream([1, 2, 3, 4, 5]);
        using var buffer = MemoryPool<byte>.Shared.Rent(3);

        // Act
        await stream.ReadExactlyAsync(buffer.Memory[..3]);

        // Assert
        buffer.Memory.ToArray().Should().StartWith(new byte[] { 1, 2, 3 });
    }
}
