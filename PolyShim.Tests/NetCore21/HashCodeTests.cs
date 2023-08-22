using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class HashCodeTests
{
    [Fact]
    public void Combine_One_Test()
    {
        // Act
        var eq1 = HashCode.Combine(1);
        var eq2 = HashCode.Combine(1);
        var neq = HashCode.Combine(2);

        // Assert
        eq1.Should().Be(eq2);
        eq1.Should().NotBe(neq);
    }

    [Fact]
    public void Combine_Two_Test()
    {
        // Act
        var eq1 = HashCode.Combine(1, 2);
        var eq2 = HashCode.Combine(1, 2);
        var neq = HashCode.Combine(1, 3);

        // Assert
        eq1.Should().Be(eq2);
        eq1.Should().NotBe(neq);
    }

    [Fact]
    public void Combine_Three_Test()
    {
        // Act
        var eq1 = HashCode.Combine(1, 2, 3);
        var eq2 = HashCode.Combine(1, 2, 3);
        var neq = HashCode.Combine(1, 2, 4);

        // Assert
        eq1.Should().Be(eq2);
        eq1.Should().NotBe(neq);
    }

    [Fact]
    public void Combine_Four_Test()
    {
        // Act
        var eq1 = HashCode.Combine(1, 2, 3, 4);
        var eq2 = HashCode.Combine(1, 2, 3, 4);
        var neq = HashCode.Combine(1, 2, 3, 5);

        // Assert
        eq1.Should().Be(eq2);
        eq1.Should().NotBe(neq);
    }

    [Fact]
    public void Combine_Five_Test()
    {
        // Act
        var eq1 = HashCode.Combine(1, 2, 3, 4, 5);
        var eq2 = HashCode.Combine(1, 2, 3, 4, 5);
        var neq = HashCode.Combine(1, 2, 3, 4, 6);

        // Assert
        eq1.Should().Be(eq2);
        eq1.Should().NotBe(neq);
    }

    [Fact]
    public void Combine_Six_Test()
    {
        // Act
        var eq1 = HashCode.Combine(1, 2, 3, 4, 5, 6);
        var eq2 = HashCode.Combine(1, 2, 3, 4, 5, 6);
        var neq = HashCode.Combine(1, 2, 3, 4, 5, 7);

        // Assert
        eq1.Should().Be(eq2);
        eq1.Should().NotBe(neq);
    }

    [Fact]
    public void Combine_Seven_Test()
    {
        // Act
        var eq1 = HashCode.Combine(1, 2, 3, 4, 5, 6, 7);
        var eq2 = HashCode.Combine(1, 2, 3, 4, 5, 6, 7);
        var neq = HashCode.Combine(1, 2, 3, 4, 5, 6, 8);

        // Assert
        eq1.Should().Be(eq2);
        eq1.Should().NotBe(neq);
    }

    [Fact]
    public void Combine_Eight_Test()
    {
        // Act
        var eq1 = HashCode.Combine(1, 2, 3, 4, 5, 6, 7, 8);
        var eq2 = HashCode.Combine(1, 2, 3, 4, 5, 6, 7, 8);
        var neq = HashCode.Combine(1, 2, 3, 4, 5, 6, 7, 9);

        // Assert
        eq1.Should().Be(eq2);
        eq1.Should().NotBe(neq);
    }
}
