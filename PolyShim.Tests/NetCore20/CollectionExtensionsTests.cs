using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class CollectionExtensionsTests
{
    [Fact]
    public void GetValueOrDefault_Test()
    {
        // Arrange
        var dictionary = new Dictionary<string, string>
        {
            ["a"] = "A",
            ["b"] = "B",
            ["c"] = "C"
        };

        // Act & assert
        dictionary.GetValueOrDefault("b").Should().Be("B");
        dictionary.GetValueOrDefault("d").Should().BeNull();
    }
}