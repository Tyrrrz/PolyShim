using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net90;

public class EnumerableExtensionsTests
{
    [Fact]
    public void Index_Test()
    {
        // Arrange
        var source = new[] { 42, 13, 69, 17 };

        // Act
        var result = source.Index();

        // Assert
        result.Should().Equal((0, 42), (1, 13), (2, 69), (3, 17));
    }

    [Fact]
    public void CountBy_Test()
    {
        // Arrange
        var source = new[] { 42, 13, 69, 17, 42, 69, 42, 13, 69, 17 };

        // Act
        var result = source.CountBy(x => x);

        // Assert
        result
            .Should()
            .BeEquivalentTo(
                new Dictionary<int, int>
                {
                    [42] = 3,
                    [13] = 2,
                    [69] = 3,
                    [17] = 2,
                }
            );
    }

    [Fact]
    public void AggregateBy_Test()
    {
        // Arrange
        var source = new[] { 42, 13, 69, 17, 42, 69, 42, 13, 69, 17 };

        // Act
        var result = source.AggregateBy(x => x, 0, (acc, x) => acc + x);

        // Assert
        result
            .Should()
            .BeEquivalentTo(
                new Dictionary<int, int>
                {
                    [42] = 126,
                    [13] = 26,
                    [69] = 207,
                    [17] = 34,
                }
            );
    }
}
