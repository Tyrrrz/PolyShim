using System;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class RandomNumberGeneratorTests
{
    [Fact]
    public void GetBytes_PartialArray_Test()
    {
        // Arrange
        var rng = RandomNumberGenerator.Create();
        var data = new byte[20];

        // Act
        rng.GetBytes(data, 5, 10);

        // Assert
        // First 5 bytes should be zero
        for (var i = 0; i < 5; i++)
        {
            data[i].Should().Be(0);
        }

        // Middle 10 bytes should have at least some non-zero values
        var middleSection = new byte[10];
        Array.Copy(data, 5, middleSection, 0, 10);
        middleSection.Should().Contain(b => b != 0);

        // Last 5 bytes should be zero
        for (var i = 15; i < 20; i++)
        {
            data[i].Should().Be(0);
        }

        ((IDisposable)rng).Dispose();
    }

    [Fact]
    public void GetBytes_EmptyCount_Test()
    {
        // Arrange
        var rng = RandomNumberGenerator.Create();
        var data = new byte[10];

        // Act & Assert
        // Should not throw and should leave array unchanged
        rng.GetBytes(data, 0, 0);
        data.Should().AllBeEquivalentTo(0);

        ((IDisposable)rng).Dispose();
    }

    [Fact]
    public void GetInt32_MaxValue_Test()
    {
        // Act & assert
        for (var i = 0; i < 100; i++)
        {
            var value = RandomNumberGenerator.GetInt32(10);
            value.Should().BeInRange(0, 9);
        }
    }

    [Fact]
    public void GetInt32_Range_Test()
    {
        // Act & assert
        for (var i = 0; i < 100; i++)
        {
            var value = RandomNumberGenerator.GetInt32(10, 20);
            value.Should().BeInRange(10, 19);
        }
    }

    [Fact]
    public void GetInt32_InvalidRange_Test()
    {
        // Act & assert
        Assert.Throws<ArgumentException>(() => RandomNumberGenerator.GetInt32(20, 10));
        Assert.Throws<ArgumentOutOfRangeException>(() => RandomNumberGenerator.GetInt32(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => RandomNumberGenerator.GetInt32(-1));
    }

    [Fact]
    public void GetBytes_Static_Test()
    {
        // Act
        var data = RandomNumberGenerator.GetBytes(16);

        // Assert
        data.Should().HaveCount(16);
        data.Should().Contain(b => b != 0);
    }

    [Fact]
    public void GetNonZeroBytes_Array_Test()
    {
        // Arrange
        var rng = RandomNumberGenerator.Create();
        var data = new byte[10];

        // Act
        rng.GetNonZeroBytes(data);

        // Assert
        data.Should().NotContain((byte)0);

        ((IDisposable)rng).Dispose();
    }

    [Fact]
    public void GetNonZeroBytes_Span_Test()
    {
        // Arrange
        var rng = RandomNumberGenerator.Create();
        var data = new Span<byte>(new byte[10]);

        // Act
        rng.GetNonZeroBytes(data);

        // Assert
        data.ToArray().Should().NotContain((byte)0);

        ((IDisposable)rng).Dispose();
    }
}
