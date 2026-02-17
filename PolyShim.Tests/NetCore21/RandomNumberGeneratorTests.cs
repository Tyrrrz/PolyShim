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
        // Verify that not all bytes are zero (cryptographic RNG should produce non-zero data)
        // Note: Theoretically, a crypto RNG could produce all zeros (probability: 2^-128),
        // but this is astronomically unlikely in practice
        buffer.Should().NotBeEquivalentTo(new byte[16]);
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
