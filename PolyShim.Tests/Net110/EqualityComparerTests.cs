using System;
using System.Collections.Generic;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class EqualityComparerTests
{
    [Fact]
    public void Create_KeySelectorComparerUsed_Test()
    {
        // Arrange
        var evenOddComparer = EqualityComparer<int>.Create(
            equals: (x, y) => (x % 2) == (y % 2),
            getHashCode: x => x % 2
        );

        var comparer = EqualityComparer<string>.Create(
            keySelector: str => str?.Length ?? 0,
            keyComparer: evenOddComparer
        );

        // Act & Assert
        comparer.Equals("12", "1234").Should().BeTrue();
        comparer.Equals("1", "123").Should().BeTrue();
        comparer.Equals("12", "1").Should().BeFalse();
        comparer.GetHashCode("12").Should().Be(0);
        comparer.GetHashCode("1").Should().Be(1);
    }
}
