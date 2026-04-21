using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class SHA1Tests
{
    [Fact]
    public void HashData_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = SHA1.HashData(data);

        // Assert
        // SHA1([1, 2, 3, 4, 5])
        hash.Should()
            .Equal(
                [0x11, 0x96, 0x6a, 0xb9, 0xc0, 0x99, 0xf8, 0xfa, 0xbe, 0xfa, 0xc5, 0x4c, 0x08, 0xd5, 0xbe, 0x2b, 0xd8, 0xc9, 0x03, 0xaf]
            );
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
        // SHA1([1, 2, 3, 4, 5])
        destination.Should()
            .Equal(
                [0x11, 0x96, 0x6a, 0xb9, 0xc0, 0x99, 0xf8, 0xfa, 0xbe, 0xfa, 0xc5, 0x4c, 0x08, 0xd5, 0xbe, 0x2b, 0xd8, 0xc9, 0x03, 0xaf]
            );
    }
}
