using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class MD5Tests
{
    [Fact]
    public void HashData_Array_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = MD5.HashData(data);

        // Assert
        hash.Should().HaveCount(16);
        hash.Should().Equal(MD5.HashData(data));
    }

    [Fact]
    public void HashData_Span_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = MD5.HashData(data.AsSpan());

        // Assert
        hash.Should().HaveCount(16);
        hash.Should().Equal(MD5.HashData(data));
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
        destination.Should().Equal(MD5.HashData(data));
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
        destination.Should().Equal(MD5.HashData(data));
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
