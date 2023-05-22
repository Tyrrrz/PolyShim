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

        // Act & assert
        source.Order().Should().Equal(13, 17, 42, 69);
    }

    [Fact]
    public void OrderDescending_Test()
    {
        // Arrange
        var source = new[] { 42, 13, 69, 17 };

        // Act & assert
        source.OrderDescending().Should().Equal(69, 42, 17, 13);
    }
}