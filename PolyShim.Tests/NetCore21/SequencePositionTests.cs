using System;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore21;

public class SequencePositionTests
{
    [Fact]
    public void Constructor_Test()
    {
        // Arrange
        var token = new object();

        // Act
        var position = new SequencePosition(token, 42);

        // Assert
        position.GetObject().Should().BeSameAs(token);
        position.GetInteger().Should().Be(42);
    }

    [Fact]
    public void Equality_Test()
    {
        // Arrange
        var token = new object();
        var a = new SequencePosition(token, 7);
        var b = new SequencePosition(token, 7);
        var c = new SequencePosition(new object(), 7);

        // Act & Assert
        a.Equals(b).Should().BeTrue();
        a.Equals(c).Should().BeFalse();
        a.Equals((object)b).Should().BeTrue();
        a.Equals((object)c).Should().BeFalse();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }
}
