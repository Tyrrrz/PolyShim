using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net50;

public class DictionaryTests
{
    [Fact]
    public void EnsureCapacity_HigherCapacity_Test()
    {
        // Arrange
        var dict = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };

        // Act
        var newCapacity = dict.EnsureCapacity(100);

        // Assert
        newCapacity.Should().BeGreaterThanOrEqualTo(100);
        dict.Should().ContainKey("a");
        dict.Should().ContainKey("b");
    }
}
