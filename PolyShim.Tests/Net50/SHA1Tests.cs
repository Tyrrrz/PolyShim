using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class SHA1Tests
{
    // SHA1([1, 2, 3, 4, 5])
    private static readonly byte[] ExpectedHash =
    [
        0x11,
        0x96,
        0x6a,
        0xb9,
        0xc0,
        0x99,
        0xf8,
        0xfa,
        0xbe,
        0xfa,
        0xc5,
        0x4c,
        0x08,
        0xd5,
        0xbe,
        0x2b,
        0xd8,
        0xc9,
        0x03,
        0xaf,
    ];

    [Fact]
    public void HashData_Array_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = SHA1.HashData(data);

        // Assert
        hash.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void HashData_Span_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = SHA1.HashData(data.AsSpan());

        // Assert
        hash.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void HashData_Span_WithDestination_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[20];

        // Act
        var bytesWritten = SHA1.HashData(data.AsSpan(), destination.AsSpan());

        // Assert
        bytesWritten.Should().Be(20);
        destination.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void HashData_Span_WithDestination_TooSmall_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[10];

        // Act & assert
        Assert.Throws<ArgumentException>(() => SHA1.HashData(data.AsSpan(), destination.AsSpan()));
    }

    [Fact]
    public void TryHashData_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[20];

        // Act
        var result = SHA1.TryHashData(data.AsSpan(), destination.AsSpan(), out var bytesWritten);

        // Assert
        result.Should().BeTrue();
        bytesWritten.Should().Be(20);
        destination.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void TryHashData_TooSmall_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[10];

        // Act
        var result = SHA1.TryHashData(data.AsSpan(), destination.AsSpan(), out var bytesWritten);

        // Assert
        result.Should().BeFalse();
        bytesWritten.Should().Be(0);
    }
}
