using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class RandomNumberGeneratorTests
{
    [Fact]
    public void Fill_Span_Test()
    {
        // Arrange
        var buffer = new byte[16];
        var span = new Span<byte>(buffer);

        // Act
        RandomNumberGenerator.Fill(span);

        // Assert
        // Since cryptographic random data is non-deterministic,
        // we can only verify that the buffer contains some non-zero bytes
        buffer.Should().Contain(b => b != 0);
    }

    [Fact]
    public void Fill_EmptySpan_Test()
    {
        // Arrange
        var buffer = new byte[0];
        var span = new Span<byte>(buffer);

        // Act & Assert
        // Should not throw and should return immediately
        RandomNumberGenerator.Fill(span);
    }
}
