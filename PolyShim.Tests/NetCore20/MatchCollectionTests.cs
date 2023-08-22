using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore20;

public class MatchCollectionTests
{
    [Fact]
    public void AsEnumerable_Test()
    {
        // Arrange
        var matches = Regex.Matches("a b c", @"\w+");

        // Act
        var enumerable = matches.AsEnumerable();

        // Assert
        // ReSharper disable once RedundantEnumerableCastCall
        enumerable.Should().Equal(matches.Cast<Match>());
    }

    [Fact]
    public void ToArray_Test()
    {
        // Arrange
        var matches = Regex.Matches("a b c", @"\w+");

        // Act
        var array = matches.ToArray();

        // Assert
        // ReSharper disable once RedundantEnumerableCastCall
        array.Should().Equal(matches.Cast<Match>());
    }
}
