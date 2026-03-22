using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class HashSetTests
{
    [Fact]
    public void EnsureCapacity_HigherCapacity_Test()
    {
        // Arrange
        var set = new HashSet<int> { 1, 2, 3 };

        // Act
        var newCapacity = set.EnsureCapacity(100);

        // Assert
        newCapacity.Should().BeGreaterThanOrEqualTo(100);
        set.Should().Contain(1);
        set.Should().Contain(2);
        set.Should().Contain(3);
    }

    [Fact]
    public void EnsureCapacity_NegativeCapacity_Test()
    {
        // Arrange
        var set = new HashSet<int> { 1, 2, 3 };

        // Act
        var act = () => set.EnsureCapacity(-1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
