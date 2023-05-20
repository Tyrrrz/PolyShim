using System.Linq;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class EnumerableExtensionsTests
{
    [Fact]
    public void Order_Test()
    {
        // Arrange
        var source = new[] { 42, 13, 69, 17 };

        // Act
        var result = source.Order();

        // Assert
        result.Should().Equal(13, 17, 42, 69);
    }

    [Fact]
    public void OrderDescending_Test()
    {
        // Arrange
        var source = new[] { 42, 13, 69, 17 };

        // Act
        var result = source.OrderDescending();

        // Assert
        result.Should().Equal(69, 42, 17, 13);
    }
}