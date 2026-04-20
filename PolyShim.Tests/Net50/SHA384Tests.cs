using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class SHA384Tests
{
    [Fact]
    public void HashData_Array_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = SHA384.HashData(data);

        // Assert
        hash.Should().HaveCount(48);
        hash.Should().Equal(SHA384.HashData(data));
    }

    [Fact]
    public void HashData_Span_Test()
    {
        // Arrange
        var data = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        var hash = SHA384.HashData(data.AsSpan());

        // Assert
        hash.Should().HaveCount(48);
        hash.Should().Equal(SHA384.HashData(data));
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
        destination.Should().Equal(SHA384.HashData(data));
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
        destination.Should().Equal(SHA384.HashData(data));
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
