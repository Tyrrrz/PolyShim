using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class IndexTests
{
    [Fact]
    public void Indexer_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        array[new Index(2)].Should().Be(3);
        array[^2].Should().Be(4);
    }

    [Fact]
    public void Equals_Test()
    {
        // Arrange
        var a = new Index(3);
        var b = new Index(3);
        var c = new Index(3, fromEnd: true);

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
        var a = new Index(5);
        var b = new Index(5);
        var c = new Index(5, fromEnd: true);

        // Act & Assert
        a.GetHashCode().Should().Be(b.GetHashCode());
        a.GetHashCode().Should().NotBe(c.GetHashCode());
    }

    [Fact]
    public void ToString_Test()
    {
        // Act & Assert
        new Index(0)
            .ToString()
            .Should()
            .Be("0");
        new Index(7).ToString().Should().Be("7");
        new Index(3, fromEnd: true).ToString().Should().Be("^3");
        new Index(0, fromEnd: true).ToString().Should().Be("^0");
    }

    [Fact]
    public void Start_Test()
    {
        // Act
        var start = Index.Start;

        // Assert
        start.Value.Should().Be(0);
        start.IsFromEnd.Should().BeFalse();
        start.ToString().Should().Be("0");
        start.Equals(new Index(0)).Should().BeTrue();
    }

    [Fact]
    public void End_Test()
    {
        // Act
        var end = Index.End;

        // Assert
        end.Value.Should().Be(0);
        end.IsFromEnd.Should().BeTrue();
        end.ToString().Should().Be("^0");
        end.Equals(new Index(0, fromEnd: true)).Should().BeTrue();
    }
}
