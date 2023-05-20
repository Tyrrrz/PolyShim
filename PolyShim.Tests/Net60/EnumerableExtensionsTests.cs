using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class EnumerableExtensionsTests
{
    [Fact]
    public void MinBy_Test()
    {
        // Arrange
        var source = new[]
        {
            new KeyValuePair<string, int>("Foo", 42),
            new KeyValuePair<string, int>("Bar", 13),
            new KeyValuePair<string, int>("Baz", 69),
            new KeyValuePair<string, int>("Qux", 17)
        };

        // Act
        var result = source.MinBy(x => x.Value);

        // Assert
        result.Key.Should().Be("Bar");
        result.Value.Should().Be(13);
    }

    [Fact]
    public void MaxBy_Test()
    {
        // Arrange
        var source = new[]
        {
            new KeyValuePair<string, int>("Foo", 42),
            new KeyValuePair<string, int>("Bar", 13),
            new KeyValuePair<string, int>("Baz", 69),
            new KeyValuePair<string, int>("Qux", 17)
        };

        // Act
        var result = source.MaxBy(x => x.Value);

        // Assert
        result.Key.Should().Be("Baz");
        result.Value.Should().Be(69);
    }

    [Fact]
    public void DistinctBy_Test()
    {
        // Arrange
        var source = new[]
        {
            new KeyValuePair<string, int>("Foo", 42),
            new KeyValuePair<string, int>("Bar", 13),
            new KeyValuePair<string, int>("Foo", 39),
            new KeyValuePair<string, int>("Qux", 17),
            new KeyValuePair<string, int>("Bar", 15),
            new KeyValuePair<string, int>("Baz", 69),
            new KeyValuePair<string, int>("Qux", 11),
            new KeyValuePair<string, int>("Baz", 54)
        };

        // Act
        var result = source.DistinctBy(x => x.Key);

        // Assert
        result.Should().Equal(
            new KeyValuePair<string, int>("Foo", 42),
            new KeyValuePair<string, int>("Bar", 13),
            new KeyValuePair<string, int>("Qux", 17),
            new KeyValuePair<string, int>("Baz", 69)
        );
    }

    [Fact]
    public void ExceptBy_Test()
    {
        // Arrange
        var source = new[]
        {
            new KeyValuePair<string, int>("Foo", 42),
            new KeyValuePair<string, int>("Bar", 13),
            new KeyValuePair<string, int>("Baz", 69),
            new KeyValuePair<string, int>("Qux", 17)
        };

        var other = new[]
        {
            "Bar",
            "Qux"
        };

        // Act
        var result = source.ExceptBy(other, x => x.Key);

        // Assert
        result.Should().Equal(
            new KeyValuePair<string, int>("Foo", 42),
            new KeyValuePair<string, int>("Baz", 69)
        );
    }

    [Fact]
    public void IntersectBy_Test()
    {
        // Arrange
        var source = new[]
        {
            new KeyValuePair<string, int>("Foo", 42),
            new KeyValuePair<string, int>("Bar", 13),
            new KeyValuePair<string, int>("Baz", 69),
            new KeyValuePair<string, int>("Qux", 17)
        };

        var other = new[]
        {
            "Bar",
            "Qux"
        };

        // Act
        var result = source.IntersectBy(other, x => x.Key);

        // Assert
        result.Should().Equal(
            new KeyValuePair<string, int>("Bar", 13),
            new KeyValuePair<string, int>("Qux", 17)
        );
    }

    [Fact]
    public void UnionBy_Test()
    {
        // Arrange
        var source = new[]
        {
            new KeyValuePair<string, int>("Foo", 42),
            new KeyValuePair<string, int>("Bar", 13),
            new KeyValuePair<string, int>("Baz", 69),
            new KeyValuePair<string, int>("Qux", 17)
        };

        var other = new[]
        {
            new KeyValuePair<string, int>("Bar", 15),
            new KeyValuePair<string, int>("Qux", 11),
            new KeyValuePair<string, int>("Baz", 54)
        };

        // Act
        var result = source.UnionBy(other, x => x.Key);

        // Assert
        result.Should().Equal(
            new KeyValuePair<string, int>("Foo", 42),
            new KeyValuePair<string, int>("Bar", 13),
            new KeyValuePair<string, int>("Baz", 69),
            new KeyValuePair<string, int>("Qux", 17)
        );
    }

    [Fact]
    public void Chunk_Test()
    {
        // Arrange
        var source = Enumerable.Range(1, 10);

        // Act
        var result = source.Chunk(3);

        // Assert
        result.Should().BeEquivalentTo(new[]
        {
            new[] { 1, 2, 3 },
            new[] { 4, 5, 6 },
            new[] { 7, 8, 9 },
            new[] { 10 }
        });
    }
}