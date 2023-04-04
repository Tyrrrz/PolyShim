using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class CollectionExtensionsTests
{
    [Fact]
    public void GetValueOrDefault_Positive_Test()
    {
        // Arrange
        var dictionary = new Dictionary<string, string>
        {
            ["a"] = "A",
            ["b"] = "B",
            ["c"] = "C"
        };

        // Act
        var value = dictionary.GetValueOrDefault("b");

        // Assert
        value.Should().Be("B");
    }

    [Fact]
    public void GetValueOrDefault_Negative_Test()
    {
        // Arrange
        var dictionary = new Dictionary<string, string>
        {
            ["a"] = "A",
            ["b"] = "B",
            ["c"] = "C"
        };

        // Act
        var value = dictionary.GetValueOrDefault("d");

        // Assert
        value.Should().BeNull();
    }
}