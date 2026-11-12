using System;
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

    [Fact]
    public void FullJoin_EmptyFirstArray_PreservesSecondOrder_Test()
    {
        // Arrange
        var first = Array.Empty<int>();
        var second = new[] { 1, 2, 1 };
        var secondKeySelectorCalls = 0;

        // Act
        var result = first
            .FullJoin(
                second,
                f => f,
                s =>
                {
                    secondKeySelectorCalls++;
                    return s;
                },
                (f, s) => $"-{s}"
            )
            .ToArray();

        // Assert
        result.Should().Equal("-1", "-2", "-1");
        secondKeySelectorCalls.Should().Be(0);
    }

    [Fact]
    public void FullJoin_NullKeys_DoNotMatch_Test()
    {
        // Arrange
        var first = new[] { ("a", null), ("b", "k") };
        var second = new[] { ("x", null), ("y", "k") };

        // Act
        var result = first
            .FullJoin(
                second,
                f => f.Item2,
                s => s.Item2,
                (f, s) => $"{f.Item1 ?? "-"}{s.Item1 ?? "-"}"
            )
            .ToArray();

        // Assert
        result.Should().Equal("a-", "by", "-x");
    }
}
