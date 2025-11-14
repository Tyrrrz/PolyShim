using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class EnumerableExtensionsTests
{
    [Fact]
    public void ElementAt_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        source.ElementAt(new Index(2)).Should().Be(3);
        source.ElementAt(^2).Should().Be(4);
    }

    [Fact]
    public void ElementAtOrDefault_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        source.ElementAtOrDefault(new Index(2)).Should().Be(3);
        source.ElementAtOrDefault(new Index(10)).Should().Be(0);
        source.ElementAtOrDefault(^2).Should().Be(4);
        source.ElementAtOrDefault(^10).Should().Be(0);
    }

    [Fact]
    public void FirstOrDefault_Test()
    {
        // Act & assert
        new[] { 1, 2, 3 }
            .FirstOrDefault(69)
            .Should()
            .Be(1);
        Array.Empty<int>().FirstOrDefault(69).Should().Be(69);
    }

    [Fact]
    public void FirstOrDefault_Predicate_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        source.FirstOrDefault(x => x % 2 == 0, 69).Should().Be(2);
        source.FirstOrDefault(x => x > 5, 69).Should().Be(69);
    }

    [Fact]
    public void LastOrDefault_Test()
    {
        // Act & assert
        new[] { 1, 2, 3 }
            .LastOrDefault(69)
            .Should()
            .Be(3);
        Array.Empty<int>().LastOrDefault(69).Should().Be(69);
    }

    [Fact]
    public void LastOrDefault_Predicate_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        source.LastOrDefault(x => x % 2 == 0, 69).Should().Be(4);
        source.LastOrDefault(x => x > 5, 69).Should().Be(69);
    }

    [Fact]
    public void SingleOrDefault_Test()
    {
        // Act & assert
        new[] { 1 }
            .SingleOrDefault(69)
            .Should()
            .Be(1);
        Array.Empty<int>().SingleOrDefault(69).Should().Be(69);
    }

    [Fact]
    public void SingleOrDefault_Predicate_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        source.SingleOrDefault(x => x % 3 == 0, 69).Should().Be(3);
        source.SingleOrDefault(x => x > 5, 69).Should().Be(69);
    }

    [Fact]
    public void Take_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        source.Take(2..^1).Should().Equal(3, 4);
    }

    [Fact]
    public void Min_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        source.Min(Comparer<int>.Default).Should().Be(1);
    }

    [Fact]
    public void MinBy_Test()
    {
        // Arrange
        var source = new[]
        {
            new KeyValuePair<string, int>("Foo", 42),
            new KeyValuePair<string, int>("Bar", 13),
            new KeyValuePair<string, int>("Baz", 69),
            new KeyValuePair<string, int>("Qux", 17),
        };

        // Act
        var result = source.MinBy(x => x.Value);

        // Assert
        result.Key.Should().Be("Bar");
        result.Value.Should().Be(13);
    }

    [Fact]
    public void Max_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        source.Max(Comparer<int>.Default).Should().Be(5);
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
            new KeyValuePair<string, int>("Qux", 17),
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
            new KeyValuePair<string, int>("Baz", 54),
        };

        // Act & assert
        source
            .DistinctBy(x => x.Key)
            .Should()
            .Equal(
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
            new KeyValuePair<string, int>("Qux", 17),
        };

        var other = new[] { "Bar", "Qux" };

        // Act & assert
        source
            .ExceptBy(other, x => x.Key)
            .Should()
            .Equal(
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
            new KeyValuePair<string, int>("Qux", 17),
        };

        var other = new[] { "Bar", "Qux" };

        // Act & assert
        source
            .IntersectBy(other, x => x.Key)
            .Should()
            .Equal(
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
            new KeyValuePair<string, int>("Qux", 17),
        };

        var other = new[]
        {
            new KeyValuePair<string, int>("Bar", 15),
            new KeyValuePair<string, int>("Qux", 11),
            new KeyValuePair<string, int>("Baz", 54),
        };

        // Act & assert
        source
            .UnionBy(other, x => x.Key)
            .Should()
            .Equal(
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

        // Act & assert
        source
            .Chunk(3)
            .Should()
            .BeEquivalentTo<int[]>([
                [1, 2, 3],
                [4, 5, 6],
                [7, 8, 9],
                [10],
            ]);
    }
}
