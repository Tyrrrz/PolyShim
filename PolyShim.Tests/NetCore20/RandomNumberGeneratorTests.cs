using System;
using System.Linq;
using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class RandomNumberGeneratorTests
{
    [Fact]
    public void GetBytes_PartialArray_Test()
    {
        // Arrange
        using var rng = RandomNumberGenerator.Create();
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
        data.Skip(5).Take(10).Should().Contain(b => b != 0);

        // Last 5 bytes should be zero
        for (var i = 15; i < 20; i++)
        {
            data[i].Should().Be(0);
        }
    }

    [Fact]
    public void GetBytes_EmptyCount_Test()
    {
        // Arrange
        using var rng = RandomNumberGenerator.Create();
        var data = new byte[10];

        // Act & Assert
        // Should not throw and should leave array unchanged
        rng.GetBytes(data, 0, 0);
        data.Should().AllBeEquivalentTo(0);
    }

    [Fact]
    public void GetNonZeroBytes_Array_Test()
    {
        // Arrange
        using var rng = RandomNumberGenerator.Create();
        var data = new byte[10];

        // Act
        rng.GetNonZeroBytes(data);

        // Assert
        data.Should().NotContain((byte)0);
    }
}
