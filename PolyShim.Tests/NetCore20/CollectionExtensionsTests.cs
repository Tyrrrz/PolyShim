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
            ["c"] = "C",
        };

        // Act & assert
        dictionary.GetValueOrDefault("b").Should().Be("B");
        dictionary.GetValueOrDefault("d").Should().BeNull();
    }

    [Fact]
    public void Remove_IDictionary_Test()
    {
        // Arrange
        var dictionary =
            (IDictionary<string, int>)new Dictionary<string, int> { ["apple"] = 1, ["banana"] = 2 };

        // Act
        var result = dictionary.Remove("apple", out var value);

        // Assert
        result.Should().BeTrue();
        value.Should().Be(1);
        dictionary.Should().NotContainKey("apple");
        dictionary.Should().HaveCount(1);
    }

    [Fact]
    public void Remove_IDictionary_NonExistent_Test()
    {
        // Arrange
        var dictionary = (IDictionary<string, int>)new Dictionary<string, int> { ["apple"] = 1 };

        // Act
        var result = dictionary.Remove("cherry", out var value);

        // Assert
        result.Should().BeFalse();
        value.Should().Be(default);
        dictionary.Should().HaveCount(1);
    }

    [Fact]
    public void TryAdd_IDictionary_Test()
    {
        // Arrange
        var dictionary =
            (IDictionary<string, int>)new Dictionary<string, int> { ["apple"] = 1, ["banana"] = 2 };

        // Act
        var result = dictionary.TryAdd("cherry", 3);

        // Assert
        result.Should().BeTrue();
        dictionary.Should().ContainKey("cherry");
        dictionary["cherry"].Should().Be(3);
        dictionary.Should().HaveCount(3);
    }

    [Fact]
    public void TryAdd_IDictionary_Exists_Test()
    {
        // Arrange
        var dictionary =
            (IDictionary<string, int>)new Dictionary<string, int> { ["apple"] = 1, ["banana"] = 2 };

        // Act
        var result = dictionary.TryAdd("apple", 99);

        // Assert
        result.Should().BeFalse();
        dictionary["apple"].Should().Be(1);
        dictionary.Should().HaveCount(2);
    }
}
