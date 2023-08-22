using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class KeyValuePairTests
{
    [Fact]
    public void Deconstruct_Test()
    {
        // Arrange
        var pair = new KeyValuePair<string, int>("hello world", 42);

        // Act
        var (key, value) = pair;

        // Assert
        key.Should().Be("hello world");
        value.Should().Be(42);
    }
}
