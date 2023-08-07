using System;
using System.Buffers;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class RandomTests
{
    [Fact]
    public void NextBytes_Span_Test()
    {
        // Arrange
        var buffer = ArrayPool<byte>.Shared.Rent(10);

        try
        {
            // Act
            new Random(1234567).NextBytes(buffer.AsSpan());

            // Assert
            buffer.Should().StartWith(new byte[] { 0x30, 0xE6, 0x63, 0xCA, 0x5F, 0x41, 0x21, 0x48, 0x1F, 0x8F });
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}