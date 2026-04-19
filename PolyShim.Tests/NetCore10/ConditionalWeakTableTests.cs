using System.Runtime.CompilerServices;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore10;

public class ConditionalWeakTableTests
{
    [Fact]
    public void GetValue_New_Test()
    {
        // Arrange
        var table = new ConditionalWeakTable<object, string>();
        var key = new object();

        // Act
        var value = table.GetValue(key, _ => "hello");

        // Assert
        value.Should().Be("hello");
    }

    [Fact]
    public void GetValue_Existing_Test()
    {
        // Arrange
        var table = new ConditionalWeakTable<object, string>();
        var key = new object();
        table.GetValue(key, _ => "hello");

        // Act
        var value = table.GetValue(key, _ => "world");

        // Assert
        value.Should().Be("hello");
    }

    [Fact]
    public void TryGetValue_Existing_Test()
    {
        // Arrange
        var table = new ConditionalWeakTable<object, string>();
        var key = new object();
        table.Add(key, "hello");

        // Act & Assert
        table.TryGetValue(key, out var value).Should().BeTrue();
        value.Should().Be("hello");
    }

    [Fact]
    public void TryGetValue_Missing_Test()
    {
        // Arrange
        var table = new ConditionalWeakTable<object, string>();
        var key = new object();

        // Act & Assert
        table.TryGetValue(key, out _).Should().BeFalse();
    }

    [Fact]
    public void Remove_Existing_Test()
    {
        // Arrange
        var table = new ConditionalWeakTable<object, string>();
        var key = new object();
        table.Add(key, "hello");

        // Act & Assert
        table.Remove(key).Should().BeTrue();
        table.TryGetValue(key, out _).Should().BeFalse();
    }

    [Fact]
    public void Remove_Missing_Test()
    {
        // Arrange
        var table = new ConditionalWeakTable<object, string>();
        var key = new object();

        // Act & Assert
        table.Remove(key).Should().BeFalse();
    }
}
