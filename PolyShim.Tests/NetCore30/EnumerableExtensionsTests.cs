using System.Linq;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class EnumerableExtensionsTests
{
    [Fact]
    public void Zip_Test()
    {
        // Arrange
        var left = new[] { 1, 2, 3 };
        var right = new[] { "a", "b", "c" };

        // Act
        var result = left.Zip(right, (l, r) => (l, r));

        // Assert
        result.Should().Equal(
            (1, "a"),
            (2, "b"),
            (3, "c")
        );
    }
}