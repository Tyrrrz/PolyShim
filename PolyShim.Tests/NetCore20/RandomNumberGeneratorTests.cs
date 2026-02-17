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
        // Act & assert
        for (var i = 0; i < 100; i++)
        {
            using var rng = RandomNumberGenerator.Create();
            var data = new byte[20];

            rng.GetBytes(data, 5, 10);

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

        // Act & Assert
        rng.GetBytes(data, 0, 0);
        data.Should().OnlyContain(b => b == 0);
    }

    [Fact]
    public void GetNonZeroBytes_Array_Test()
    {
        // Act & assert
        for (var i = 0; i < 100; i++)
        {
            using var rng = RandomNumberGenerator.Create();
            var data = new byte[10];

            rng.GetNonZeroBytes(data);

            data.Should().NotContain((byte)0);
        }
    }
}
