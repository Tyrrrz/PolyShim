using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class DictionaryTests
{
    [Fact]
    public void TryAdd_Test()
    {
        // Arrange
        var dictionary = new Dictionary<string, int> { ["apple"] = 1, ["banana"] = 2 };

        // Act
        var result = dictionary.TryAdd("cherry", 3);

        // Assert
        result.Should().BeTrue();
        dictionary.Should().ContainKey("cherry");
        dictionary["cherry"].Should().Be(3);
        dictionary.Should().HaveCount(3);
    }

    [Fact]
    public void TryAdd_Exists_Test()
    {
        // Arrange
        var dictionary = new Dictionary<string, int> { ["apple"] = 1, ["banana"] = 2 };

        // Act
        var result = dictionary.TryAdd("apple", 99);

        // Assert
        result.Should().BeFalse();
        dictionary["apple"].Should().Be(1);
        dictionary.Should().HaveCount(2);
    }
}
