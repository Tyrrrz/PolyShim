using System.Collections;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class DictionaryEntryTests
{
    [Fact]
    public void Deconstruct_Test()
    {
        // Arrange
        var pair = new DictionaryEntry("hello world", 42);

        // Act
        var (key, value) = pair;

        // Assert
        key.Should().Be("hello world");
        value.Should().Be(42);
    }
}
