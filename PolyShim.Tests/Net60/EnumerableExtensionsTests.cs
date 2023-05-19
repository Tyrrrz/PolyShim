using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net60;

public class EnumerableExtensionsTests
{
    [Fact]
    public void MinBy_Test()
    {
        // Arrange
        var source = new[]
        {
            new KeyValuePair<string, int>("Foo", 42),
            new KeyValuePair<string, int>("Bar", 13),
            new KeyValuePair<string, int>("Baz", 69),
            new KeyValuePair<string, int>("Qux", 17)
        };

        // Act
        var result = source.MinBy(x => x.Value);

        // Assert
        result.Key.Should().Be("Bar");
        result.Value.Should().Be(13);
    }

    [Fact]
    public void MaxBy_Test()
    {
        // Arrange
        var source = new[]
        {
            new KeyValuePair<string, int>("Foo", 42),
            new KeyValuePair<string, int>("Bar", 13),
            new KeyValuePair<string, int>("Baz", 69),
            new KeyValuePair<string, int>("Qux", 17)
        };

        // Act
        var result = source.MaxBy(x => x.Value);

        // Assert
        result.Key.Should().Be("Baz");
        result.Value.Should().Be(69);
    }
}