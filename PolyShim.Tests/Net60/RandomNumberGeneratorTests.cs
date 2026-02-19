using System.Security.Cryptography;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class RandomNumberGeneratorTests
{
    [Fact]
    public void GetBytes_Test()
    {
        for (var i = 0; i < 100; i++)
        {
            // Act
            var data = RandomNumberGenerator.GetBytes(16);

            // Assert
            data.Should().HaveCount(16);
            data.Should().Contain(b => b != 0);
        }
    }
}
