using System.Linq;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class EnumerableExtensionsTests
{
    [Fact]
    public void ToHashSet_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 2, 3, 3, 3, 4, 4, 4, 5 };

        // Act
        var result = source.ToHashSet();

        // Assert
        result.Should().Equal(1, 2, 3, 4, 5);
    }
}