using System;
using System.Linq;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net100;

public class EnumerableExtensionsTests
{
    [Fact]
    public void Shuffle_Test()
    {
        // Arrange
        var source = Enumerable.Range(0, 100).ToArray();

        // Act
        var result = source.Shuffle().ToArray();

        // Assert
        result.Should().BeEquivalentTo(source);
        result.Should().NotEqual(source);
        Array.Empty<int>().Shuffle().Should().BeEmpty();
    }
}
