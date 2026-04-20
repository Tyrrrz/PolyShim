using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class SHA256Tests
{
    // SHA256([1, 2, 3, 4, 5])
    private static readonly byte[] ExpectedHash =
    [
        0x74,
        0xf8,
        0x1f,
        0xe1,
        0x67,
        0xd9,
        0x9b,
        0x4c,
        0xb4,
        0x1d,
        0x6d,
        0x0c,
        0xcd,
        0xa8,
        0x22,
        0x78,
        0xca,
        0xee,
        0x9f,
        0x3e,
        0x2f,
        0x25,
        0xd5,
        0xe5,
        0xa3,
        0x93,
        0x6f,
        0xf3,
        0xdc,
        0xec,
        0x60,
        0xd0,
    ];

    [Fact]
    public void HashData_Array_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = SHA256.HashData(data);

        // Assert
        hash.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void HashData_Span_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = SHA256.HashData(data.AsSpan());

        // Assert
        hash.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void HashData_Span_WithDestination_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[32];

        // Act
        var bytesWritten = SHA256.HashData(data.AsSpan(), destination.AsSpan());

        // Assert
        bytesWritten.Should().Be(32);
        destination.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void HashData_Span_WithDestination_TooSmall_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[16];

        // Act & assert
        Assert.Throws<ArgumentException>(() =>
            SHA256.HashData(data.AsSpan(), destination.AsSpan())
        );
    }

    [Fact]
    public void TryHashData_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[32];

        // Act
        var result = SHA256.TryHashData(data.AsSpan(), destination.AsSpan(), out var bytesWritten);

        // Assert
        result.Should().BeTrue();
        bytesWritten.Should().Be(32);
        destination.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void TryHashData_TooSmall_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[16];

        // Act
        var result = SHA256.TryHashData(data.AsSpan(), destination.AsSpan(), out var bytesWritten);

        // Assert
        result.Should().BeFalse();
        bytesWritten.Should().Be(0);
    }
}
