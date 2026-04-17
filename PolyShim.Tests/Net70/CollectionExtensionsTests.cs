using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net70;

public class CollectionExtensionsTests
{
    [Fact]
    public void AsReadOnly_IList_Test()
    {
        // Arrange
        IList<string> list = new List<string> { "a", "b", "c" };

        // Act
        var readOnly = list.AsReadOnly();

        // Assert
        readOnly.Should().BeOfType<ReadOnlyCollection<string>>();
        readOnly.Should().Equal("a", "b", "c");
    }

    [Fact]
    public void AsReadOnly_IList_ReflectsChanges_Test()
    {
        // Arrange
        var inner = new List<string> { "a", "b" };
        IList<string> list = inner;

        // Act
        var readOnly = list.AsReadOnly();
        inner.Add("c");

        // Assert
        readOnly.Should().Equal("a", "b", "c");
    }

    [Fact]
    public void AsReadOnly_IDictionary_Test()
    {
        // Arrange
        var dictionary =
            (IDictionary<string, int>)new Dictionary<string, int> { ["one"] = 1, ["two"] = 2 };

        // Act
        var readOnly = dictionary.AsReadOnly();

        // Assert
        readOnly.Should().BeOfType<ReadOnlyDictionary<string, int>>();
        readOnly["one"].Should().Be(1);
        readOnly["two"].Should().Be(2);
    }
}
