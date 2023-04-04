using System;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class IndexTests
{
    [Fact]
    public void Indexer_FromStart_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = array[new Index(2)];

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    public void Indexer_FromEnd_Test()
    {
        // Arrange
        var array = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = array[^2];

        // Assert
        result.Should().Be(4);
    }
}