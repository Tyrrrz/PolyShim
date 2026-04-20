using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class SHA512Tests
{
    [Fact]
    public void HashData_Array_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = SHA512.HashData(data);

        // Assert
        hash.Should().HaveCount(64);
        hash.Should().Equal(SHA512.HashData(data));
    }

    [Fact]
    public void HashData_Span_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = SHA512.HashData(data.AsSpan());

        // Assert
        hash.Should().HaveCount(64);
        hash.Should().Equal(SHA512.HashData(data));
    }

    [Fact]
    public void HashData_Span_WithDestination_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[64];

        // Act
        var bytesWritten = SHA512.HashData(data.AsSpan(), destination.AsSpan());

        // Assert
        bytesWritten.Should().Be(64);
        destination.Should().Equal(SHA512.HashData(data));
    }

    [Fact]
    public void HashData_Span_WithDestination_TooSmall_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[32];

        // Act & assert
        Assert.Throws<ArgumentException>(() =>
            SHA512.HashData(data.AsSpan(), destination.AsSpan())
        );
    }

    [Fact]
    public void TryHashData_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[64];

        // Act
        var result = SHA512.TryHashData(data.AsSpan(), destination.AsSpan(), out var bytesWritten);

        // Assert
        result.Should().BeTrue();
        bytesWritten.Should().Be(64);
        destination.Should().Equal(SHA512.HashData(data));
    }

    [Fact]
    public void TryHashData_TooSmall_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };
        var destination = new byte[32];

        // Act
        var result = SHA512.TryHashData(data.AsSpan(), destination.AsSpan(), out var bytesWritten);

        // Assert
        result.Should().BeFalse();
        bytesWritten.Should().Be(0);
    }
}
