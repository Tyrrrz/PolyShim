using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class RandomNumberGeneratorTests
{
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
}
