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
        for (var i = 0; i < 100; i++)
        {
            // Arrange
            using var rng = RandomNumberGenerator.Create();
            var data = new byte[20];

            // Act
            rng.GetBytes(data, 5, 10);

            // Assert
            data.Take(5).Should().OnlyContain(b => b == 0);
            data.Skip(5).Take(10).Should().Contain(b => b != 0);
            data.Skip(15).Should().OnlyContain(b => b == 0);
        }
    }

    [Fact]
    public void GetBytes_EmptyCount_Test()
    {
        // Arrange
        using var rng = RandomNumberGenerator.Create();
        var data = new byte[10];

        // Act
        rng.GetBytes(data, 0, 0);

        // Assert
        data.Should().OnlyContain(b => b == 0);
    }

    [Fact]
    public void GetNonZeroBytes_Array_Test()
    {
        for (var i = 0; i < 100; i++)
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
}
