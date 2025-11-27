using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class SortedSetTests
{
    [Fact]
    public void TryGetValue_Found_Test()
    {
        // Arrange
        var set = new SortedSet<string> { "apple", "banana", "cherry" };

        // Act
        var result = set.TryGetValue("banana", out var actualValue);

        // Assert
        result.Should().BeTrue();
        actualValue.Should().Be("banana");
    }

    [Fact]
    public void TryGetValue_NotFound_Test()
    {
        // Arrange
        var set = new SortedSet<string> { "apple", "banana", "cherry" };

        // Act
        var result = set.TryGetValue("orange", out var actualValue);

        // Assert
        result.Should().BeFalse();
        actualValue.Should().BeNull();
    }

    [Fact]
    public void TryGetValue_EmptySet_Test()
    {
        // Arrange
        var set = new SortedSet<string>();

        // Act
        var result = set.TryGetValue("apple", out var actualValue);

        // Assert
        result.Should().BeFalse();
        actualValue.Should().BeNull();
    }

    [Fact]
    public void TryGetValue_ValueType_Found_Test()
    {
        // Arrange
        var set = new SortedSet<int> { 1, 2, 3, 4, 5 };

        // Act
        var result = set.TryGetValue(3, out var actualValue);

        // Assert
        result.Should().BeTrue();
        actualValue.Should().Be(3);
    }

    [Fact]
    public void TryGetValue_ValueType_NotFound_Test()
    {
        // Arrange
        var set = new SortedSet<int> { 1, 2, 3, 4, 5 };

        // Act
        var result = set.TryGetValue(10, out var actualValue);

        // Assert
        result.Should().BeFalse();
        actualValue.Should().Be(0);
    }

    [Fact]
    public void TryGetValue_CustomComparer_Test()
    {
        // Arrange
        var set = new SortedSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Apple",
            "Banana",
            "Cherry",
        };

        // Act
        var result = set.TryGetValue("BANANA", out var actualValue);

        // Assert
        result.Should().BeTrue();
        actualValue.Should().Be("Banana"); // Returns the actual stored value
    }

    [Fact]
    public void TryGetValue_CustomComparer_NotFound_Test()
    {
        // Arrange
        var set = new SortedSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Apple",
            "Banana",
            "Cherry",
        };

        // Act
        var result = set.TryGetValue("Orange", out var actualValue);

        // Assert
        result.Should().BeFalse();
        actualValue.Should().BeNull();
    }

    [Fact]
    public void TryGetValue_CustomObject_Test()
    {
        // Arrange
        var obj1 = new Person { Id = 1, Name = "Alice" };
        var obj2 = new Person { Id = 2, Name = "Bob" };
        var set = new SortedSet<Person>(new PersonComparer()) { obj1, obj2 };

        var searchObj = new Person { Id = 1, Name = "Different Name" };

        // Act
        var result = set.TryGetValue(searchObj, out var actualValue);

        // Assert
        result.Should().BeTrue();
        actualValue.Should().BeSameAs(obj1);
        actualValue!.Name.Should().Be("Alice");
    }

    [Fact]
    public void TryGetValue_MaintainsSortOrder_Test()
    {
        // Arrange
        var set = new SortedSet<int> { 5, 3, 1, 4, 2 };

        // Act & Assert - verify elements are sorted
        set.Should().BeInAscendingOrder();

        // Verify TryGetValue works correctly
        var result = set.TryGetValue(3, out var actualValue);
        result.Should().BeTrue();
        actualValue.Should().Be(3);
    }

    private class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    private class PersonComparer : IComparer<Person>
    {
        public int Compare(Person? x, Person? y)
        {
            if (x is null || y is null)
                return x == y ? 0 : (x is null ? -1 : 1);
            return x.Id.CompareTo(y.Id);
        }
    }
}
