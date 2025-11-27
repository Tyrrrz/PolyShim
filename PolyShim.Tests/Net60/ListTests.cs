using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class ListTests
{
    [Fact]
    public void EnsureCapacity_IncreasesCapacity_Test()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        var newCapacity = list.EnsureCapacity(100);

        // Assert
        newCapacity.Should().Be(list.Capacity);
        list.Capacity.Should().BeGreaterOrEqualTo(100);
        list.Capacity.Should().Be(newCapacity);
        list.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void EnsureCapacity_DoesNotDecreaseCapacity_Test()
    {
        // Arrange
        var list = new List<int>(100) { 1, 2, 3 };
        var originalCapacity = list.Capacity;

        // Act
        var newCapacity = list.EnsureCapacity(10);

        // Assert
        newCapacity.Should().Be(originalCapacity);
        list.Capacity.Should().Be(originalCapacity);
        list.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void EnsureCapacity_NegativeCapacity_Test()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        var act = () => list.EnsureCapacity(-1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
        list.Should().Equal(1, 2, 3);
    }
}
