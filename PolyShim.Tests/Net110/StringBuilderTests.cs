using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class StringBuilderTests
{
    [Fact]
    public void MoveChunks_Test()
    {
        // Arrange
        var source = new StringBuilder("bar");

        // Act
        var destination = StringBuilder.MoveChunks(source);

        // Assert
        destination.ToString().Should().Be("bar");
        source.Length.Should().Be(0);
    }
}
