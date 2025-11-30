using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class RandomTests
{
    [Fact]
    public void Shared_Test()
    {
        // Act & assert
        Random.Shared.Should().NotBeNull();
        Random.Shared.Next(1, 100).Should().BeInRange(1, 99);
    }

    [Fact]
    public void NextInt64_Test()
    {
        // Act & assert
        for (var i = 0; i < 100; i++)
        {
            Random.Shared.NextInt64(10, 20).Should().BeInRange(10, 19);
            Random.Shared.NextInt64(20).Should().BeInRange(0, 19);
            Random.Shared.NextInt64().Should().BeInRange(0, long.MaxValue - 1);
        }
    }

    [Fact]
    public void NextSingle_Test()
    {
        // Act & assert
        for (var i = 0; i < 100; i++)
        {
            Random.Shared.NextSingle().Should().BeInRange(0f, 1f);
        }
    }
}
