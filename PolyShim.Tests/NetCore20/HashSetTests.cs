using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class HashSetTests
{
    [Fact]
    public void TryGetValue_Test()
    {
        // Arrange
        var set = new HashSet<string> { "apple", "banana", "cherry" };

        // Act
        var result = set.TryGetValue("banana", out var actualValue);

        // Assert
        result.Should().BeTrue();
        actualValue.Should().Be("banana");
    }

    [Fact]
    public void TryGetValue_NotFound_Test()
    {
        // Arrange
        var set = new HashSet<string> { "apple", "banana", "cherry" };

        // Act
        var result = set.TryGetValue("orange", out var actualValue);

        // Assert
        result.Should().BeFalse();
        actualValue.Should().BeNull();
    }

    [Fact]
    public void TryGetValue_CustomComparer_Test()
    {
        // Arrange
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "apple",
            "banana",
            "cherry",
        };

        // Act
        var result = set.TryGetValue("BANANA", out var actualValue);

        // Assert
        result.Should().BeTrue();
        actualValue.Should().Be("banana");
    }
}
