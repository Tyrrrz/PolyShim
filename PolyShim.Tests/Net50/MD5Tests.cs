using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class MD5Tests
{
    [Fact]
    public void HashData_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = MD5.HashData(data);

        // Assert
        // MD5([1, 2, 3, 4, 5])
        hash.Should()
            .Equal(
                [0x7c, 0xfd, 0xd0, 0x78, 0x89, 0xb3, 0x29, 0x5d, 0x6a, 0x55, 0x09, 0x14, 0xab, 0x35, 0xe0, 0x68]
            );
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
        // MD5([1, 2, 3, 4, 5])
        destination.Should()
            .Equal(
                [0x7c, 0xfd, 0xd0, 0x78, 0x89, 0xb3, 0x29, 0x5d, 0x6a, 0x55, 0x09, 0x14, 0xab, 0x35, 0xe0, 0x68]
            );
    }
}
