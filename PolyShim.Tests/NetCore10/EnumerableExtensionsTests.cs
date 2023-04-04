using System.Linq;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class EnumerableExtensionsTests
{
    [Fact]
    public void Prepend_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };

        // Act
        var result = source.Prepend(0);

        // Assert
        result.Should().Equal(0, 1, 2, 3);
    }

    [Fact]
    public void Append_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };

        // Act
        var result = source.Append(4);

        // Assert
        result.Should().Equal(1, 2, 3, 4);
    }
}