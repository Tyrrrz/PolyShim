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
        var destination = new StringBuilder("foo");
        var source = new StringBuilder("bar");

        // Act
        destination.MoveChunks(source);

        // Assert
        destination.ToString().Should().Be("foobar");
        source.Length.Should().Be(0);
    }
}
