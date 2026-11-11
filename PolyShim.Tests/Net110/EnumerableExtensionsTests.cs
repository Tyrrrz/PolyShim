using System.Linq;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class EnumerableExtensionsTests
{
    [Fact]
    public void FullJoin_Test()
    {
        // Arrange
        var first = new[] { (1, "a"), (2, "b"), (2, "c") };
        var second = new[] { (2, "x"), (3, "y"), (2, "z") };

        // Act
        var result = first
            .FullJoin(
                second,
                f => f.Item1,
                s => s.Item1,
                (f, s) => $"{f.Item2 ?? "-"}{s.Item2 ?? "-"}"
            )
            .ToArray();

        // Assert
        result.Should().Equal("a-", "bx", "bz", "cx", "cz", "-y");
    }
}
