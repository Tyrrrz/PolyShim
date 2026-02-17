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
        var span = new Span<byte>(new byte[16]);

        // Act
        RandomNumberGenerator.Fill(span);

        // Assert
        // Verify that not all bytes are zero (cryptographic RNG should produce non-zero data).
        // Note: Theoretically, a crypto RNG could produce all zeros (probability: 2^-128),
        // but this is astronomically unlikely in practice.
        span.ToArray().Should().Contain(b => b != 0);
    }

    [Fact]
    public void Fill_EmptySpan_Test()
    {
        // Arrange
        var span = new Span<byte>();

        // Act & Assert
        // Should not throw and should return immediately
        RandomNumberGenerator.Fill(span);
    }

    [Fact]
    public void GetBytes_Span_Test()
    {
        // Arrange
        using var rng = RandomNumberGenerator.Create();
        var span = new Span<byte>(new byte[16]);

        // Act
        rng.GetBytes(span);

        // Assert
        span.ToArray().Should().Contain(b => b != 0);
    }

    [Fact]
    public void GetBytes_EmptySpan_Test()
    {
        // Arrange
        using var rng = RandomNumberGenerator.Create();
        var span = new Span<byte>();

        // Act & Assert
        // Should not throw and should return immediately
        rng.GetBytes(span);
    }

    [Fact]
    public void GetNonZeroBytes_Span_Test()
    {
        // Arrange
        using var rng = RandomNumberGenerator.Create();
        var data = new Span<byte>(new byte[10]);

        // Act
        rng.GetNonZeroBytes(data);

        // Assert
        data.ToArray().Should().NotContain((byte)0);
    }
}
