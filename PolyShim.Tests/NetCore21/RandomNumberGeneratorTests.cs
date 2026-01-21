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
        Span<byte> buffer = stackalloc byte[16];

        // Act
        RandomNumberGenerator.Fill(buffer);

        // Assert
        // Since cryptographic random data is non-deterministic,
        // we can only verify that the buffer contains some non-zero bytes
        buffer.ToArray().Should().Contain(b => b != 0);
    }
}
