using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class SHA384Tests
{
    // SHA384([1, 2, 3, 4, 5])
    private static readonly byte[] ExpectedHash =
    [
        0xd8,
        0x88,
        0x75,
        0xdb,
        0x0f,
        0x77,
        0xaa,
        0xd8,
        0xf3,
        0xd9,
        0x94,
        0xfe,
        0x68,
        0xcd,
        0x1c,
        0xc7,
        0xec,
        0x3a,
        0x4f,
        0xf1,
        0x43,
        0x78,
        0xb7,
        0xfe,
        0xb9,
        0x91,
        0xe5,
        0x47,
        0x84,
        0x85,
        0x01,
        0x92,
        0x14,
        0x58,
        0x54,
        0xc3,
        0x6e,
        0x5a,
        0x40,
        0xa0,
        0xc2,
        0xe8,
        0x0d,
        0xa2,
        0x00,
        0x2d,
        0x7c,
        0xc8,
    ];

    [Fact]
    public void HashData_Array_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = SHA384.HashData(data);

        // Assert
        hash.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void HashData_Span_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = SHA384.HashData(data.AsSpan());

        // Assert
        hash.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void HashData_Span_WithDestination_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[48];

        // Act
        var bytesWritten = SHA384.HashData(data.AsSpan(), destination.AsSpan());

        // Assert
        bytesWritten.Should().Be(48);
        destination.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void HashData_Span_WithDestination_TooSmall_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[32];

        // Act & assert
        Assert.Throws<ArgumentException>(() =>
            SHA384.HashData(data.AsSpan(), destination.AsSpan())
        );
    }

    [Fact]
    public void TryHashData_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[48];

        // Act
        var result = SHA384.TryHashData(data.AsSpan(), destination.AsSpan(), out var bytesWritten);

        // Assert
        result.Should().BeTrue();
        bytesWritten.Should().Be(48);
        destination.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void TryHashData_TooSmall_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[32];

        // Act
        var result = SHA384.TryHashData(data.AsSpan(), destination.AsSpan(), out var bytesWritten);

        // Assert
        result.Should().BeFalse();
        bytesWritten.Should().Be(0);
    }
}
