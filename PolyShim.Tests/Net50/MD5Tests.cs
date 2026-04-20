using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class MD5Tests
{
    // MD5([1, 2, 3, 4, 5])
    private static readonly byte[] ExpectedHash =
    [
        0x7c,
        0xfd,
        0xd0,
        0x78,
        0x89,
        0xb3,
        0x29,
        0x5d,
        0x6a,
        0x55,
        0x09,
        0x14,
        0xab,
        0x35,
        0xe0,
        0x68,
    ];

    [Fact]
    public void HashData_Array_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = MD5.HashData(data);

        // Assert
        hash.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void HashData_Span_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = MD5.HashData(data.AsSpan());

        // Assert
        hash.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void HashData_Span_WithDestination_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[16];

        // Act
        var bytesWritten = MD5.HashData(data.AsSpan(), destination.AsSpan());

        // Assert
        bytesWritten.Should().Be(16);
        destination.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void HashData_Span_WithDestination_TooSmall_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[8];

        // Act & assert
        Assert.Throws<ArgumentException>(() => MD5.HashData(data.AsSpan(), destination.AsSpan()));
    }

    [Fact]
    public void TryHashData_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[16];

        // Act
        var result = MD5.TryHashData(data.AsSpan(), destination.AsSpan(), out var bytesWritten);

        // Assert
        result.Should().BeTrue();
        bytesWritten.Should().Be(16);
        destination.Should().Equal(ExpectedHash);
    }

    [Fact]
    public void TryHashData_TooSmall_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[8];

        // Act
        var result = MD5.TryHashData(data.AsSpan(), destination.AsSpan(), out var bytesWritten);

        // Assert
        result.Should().BeFalse();
        bytesWritten.Should().Be(0);
    }
}
