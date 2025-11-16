using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class RandomTests
{
    [Fact]
    public void Shared_Test()
    {
        // Act
        var value = Random.Shared.Next(1, 100);

        // Assert
        value.Should().BeInRange(1, 100);
    }
}
