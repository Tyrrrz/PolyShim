using System.Runtime.CompilerServices;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class ConditionalWeakTableTests
{
    [Fact]
    public void GetValue_Test()
    {
        // Arrange
        var table = new ConditionalWeakTable<object, string>();
        var missingKey = new object();
        var existingKey = new object();
        table.Add(existingKey, "hello");

        // Act
        var createdValue = table.GetValue(missingKey, _ => "world");
        var existingValue = table.GetValue(existingKey, _ => "ignored");

        // Assert
        createdValue.Should().Be("world");
        existingValue.Should().Be("hello");
    }

    [Fact]
    public void TryGetValue_Test()
    {
        // Arrange
        var table = new ConditionalWeakTable<object, string>();
        var existingKey = new object();
        var missingKey = new object();
        table.Add(existingKey, "hello");

        // Act & Assert
        table.TryGetValue(existingKey, out var existingValue).Should().BeTrue();
        existingValue.Should().Be("hello");

        table.TryGetValue(missingKey, out _).Should().BeFalse();
    }

    [Fact]
    public void Remove_Test()
    {
        // Arrange
        var table = new ConditionalWeakTable<object, string>();
        var existingKey = new object();
        var missingKey = new object();
        table.Add(existingKey, "hello");

        // Act & Assert
        table.Remove(existingKey).Should().BeTrue();
        table.TryGetValue(existingKey, out _).Should().BeFalse();

        table.Remove(missingKey).Should().BeFalse();
    }
}
