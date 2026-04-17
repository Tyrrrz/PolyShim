using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net80;

public class CollectionExtensionsTests
{
    [Fact]
    public void AddRange_Test()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        ReadOnlySpan<int> items = new[] { 4, 5, 6 };

        // Act
        list.AddRange(items);

        // Assert
        list.Should().Equal(1, 2, 3, 4, 5, 6);
    }

    [Fact]
    public void AddRange_Empty_Test()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        ReadOnlySpan<int> items = [];

        // Act
        list.AddRange(items);

        // Assert
        list.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void InsertRange_Test()
    {
        // Arrange
        var list = new List<int> { 1, 2, 6 };
        ReadOnlySpan<int> items = new[] { 3, 4, 5 };

        // Act
        list.InsertRange(2, items);

        // Assert
        list.Should().Equal(1, 2, 3, 4, 5, 6);
    }

    [Fact]
    public void CopyTo_Test()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        var destination = new int[5];

        // Act
        list.CopyTo(destination.AsSpan());

        // Assert
        destination[0].Should().Be(1);
        destination[1].Should().Be(2);
        destination[2].Should().Be(3);
    }

    [Fact]
    public void CopyTo_TooSmall_Test()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        var destination = new int[2];

        // Act
        var act = () => list.CopyTo(destination.AsSpan());

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
