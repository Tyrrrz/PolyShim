using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class EqualityComparerTests
{
    [Fact]
    public void Create_Test()
    {
        // Arrange
        var comparer = EqualityComparer<int>.Create(
            (x, y) => Math.Abs(x) == Math.Abs(y),
            x => Math.Abs(x).GetHashCode()
        );

        // Act & Assert
        comparer.Equals(5, 5).Should().BeTrue();
        comparer.Equals(5, -5).Should().BeTrue();
        comparer.Equals(5, 4).Should().BeFalse();
        comparer.GetHashCode(5).Should().Be(comparer.GetHashCode(-5));
    }

    [Fact]
    public void Create_NoGetHashCode_Test()
    {
        // Arrange
        var comparer = EqualityComparer<int>.Create((x, y) => Math.Abs(x) == Math.Abs(y));

        // Act & Assert
        comparer.Equals(5, 5).Should().BeTrue();
        comparer.Equals(5, -5).Should().BeTrue();
        comparer.Equals(5, 4).Should().BeFalse();
        Assert.Throws<NotSupportedException>(() =>
            comparer.GetHashCode(5).Should().Be(comparer.GetHashCode(-5))
        );
    }
}
