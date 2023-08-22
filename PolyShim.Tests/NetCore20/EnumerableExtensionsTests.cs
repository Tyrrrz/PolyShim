using System.Linq;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class EnumerableExtensionsTests
{
    [Fact]
    public void TakeLast_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        source.TakeLast(3).Should().Equal(3, 4, 5);
    }

    [Fact]
    public void SkipLast_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        source.SkipLast(3).Should().Equal(1, 2);
    }

    [Fact]
    public void ToHashSet_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 2, 3, 3, 3, 4, 4, 4, 5 };

        // Act & assert
        source.ToHashSet().Should().Equal(1, 2, 3, 4, 5);
    }
}
