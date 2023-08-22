using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class IndexTests
{
    [Fact]
    public void Indexer_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act & assert
        array[new Index(2)].Should().Be(3);
        array[^2].Should().Be(4);
    }
}
