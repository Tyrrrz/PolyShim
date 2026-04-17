using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.Net100;

public class CollectionExtensionsTests
{
    [Fact]
    public void AsReadOnly_ISet_Test()
    {
        // Arrange
        var set = (ISet<int>)new HashSet<int> { 1, 2, 3 };

        // Act
        var readOnly = set.AsReadOnly();

        // Assert
        readOnly.Should().BeOfType<ReadOnlySet<int>>();
        readOnly.Count.Should().Be(3);
        readOnly.Contains(1).Should().BeTrue();
        readOnly.Contains(4).Should().BeFalse();
        readOnly.IsSubsetOf(new[] { 1, 2, 3 }).Should().BeTrue();
        readOnly.IsSupersetOf(new[] { 1 }).Should().BeTrue();
    }

    [Fact]
    public void AsReadOnly_ISet_ReflectsChanges_Test()
    {
        // Arrange
        var inner = new HashSet<int> { 1, 2, 3 };
        var set = (ISet<int>)inner;

        // Act
        var readOnly = set.AsReadOnly();
        inner.Add(4);

        // Assert
        readOnly.Count.Should().Be(4);
        readOnly.Contains(4).Should().BeTrue();
    }
}
