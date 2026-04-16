using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class RangeTests
{
    [Fact]
    public void Slice_Test()
    {
        // Arrange
        const string str = "Hello world!";

        // Act & assert
        str[6..11].Should().Be("world");
        str[..5].Should().Be("Hello");
        str[6..].Should().Be("world!");
    }

    [Fact]
    public void Equals_Test()
    {
        // Arrange
        var a = new Range(new Index(2), new Index(5));
        var b = new Range(new Index(2), new Index(5));
        var c = new Range(new Index(2), new Index(7));

        // Act & Assert
        a.Equals(b).Should().BeTrue();
        a.Equals(c).Should().BeFalse();
        a.Equals((object)b).Should().BeTrue();
        a.Equals((object)c).Should().BeFalse();
        a.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_Test()
    {
        // Arrange
        var a = new Range(new Index(1), new Index(4));
        var b = new Range(new Index(1), new Index(4));
        var c = new Range(new Index(2), new Index(4));

        // Act & Assert
        a.GetHashCode().Should().Be(b.GetHashCode());
        a.GetHashCode().Should().NotBe(c.GetHashCode());
    }

    [Fact]
    public void ToString_Test()
    {
        // Act & Assert
        new Range(new Index(0), new Index(5))
            .ToString()
            .Should()
            .Be("0..5");
        new Range(new Index(3), new Index(0, fromEnd: true)).ToString().Should().Be("3..^0");
        Range.All.ToString().Should().Be("0..^0");
    }

    [Fact]
    public void StartAt_Test()
    {
        // Act
        var range = Range.StartAt(new Index(3));

        // Assert
        range.Start.Equals(new Index(3)).Should().BeTrue();
        range.End.Equals(Index.End).Should().BeTrue();
    }

    [Fact]
    public void EndAt_Test()
    {
        // Act
        var range = Range.EndAt(new Index(5));

        // Assert
        range.Start.Equals(Index.Start).Should().BeTrue();
        range.End.Equals(new Index(5)).Should().BeTrue();
    }

    [Fact]
    public void All_Test()
    {
        // Act
        var all = Range.All;

        // Assert
        all.Start.Equals(Index.Start).Should().BeTrue();
        all.End.Equals(Index.End).Should().BeTrue();
    }
}
