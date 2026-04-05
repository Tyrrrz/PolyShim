using System.Runtime.CompilerServices;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class IUnionTests
{
    private sealed class TestUnion : IUnion
    {
        public TestUnion(object? value) => Value = value;

        public object? Value { get; }
    }

    [Fact]
    public void Value_ReturnsExpectedValue_Test()
    {
        // Arrange
        var union = new TestUnion(42);

        // Act & Assert
        union.Value.Should().Be(42);
    }

    [Fact]
    public void Value_ReturnsNull_Test()
    {
        // Arrange
        var union = new TestUnion(null);

        // Act & Assert
        union.Value.Should().BeNull();
    }
}
