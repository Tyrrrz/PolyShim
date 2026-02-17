using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class RandomNumberGeneratorTests
{
    [Fact]
    public void GetInt32_MaxValue_Test()
    {
        for (var i = 0; i < 100; i++)
        {
            // Act
            var value = RandomNumberGenerator.GetInt32(10);

            // Assert
            value.Should().BeInRange(0, 9);
        }
    }

    [Fact]
    public void GetInt32_Range_Test()
    {
        for (var i = 0; i < 100; i++)
        {
            // Act
            var value = RandomNumberGenerator.GetInt32(10, 20);

            // Assert
            value.Should().BeInRange(10, 19);
        }
    }
}
