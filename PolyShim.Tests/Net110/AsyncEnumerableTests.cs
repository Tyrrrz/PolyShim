using System.Linq;
using System.Threading.Tasks;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net110;

public class AsyncEnumerableTests
{
    [Fact]
    public async Task FullJoin_Test()
    {
        // Arrange
        var first = new[] { (1, "a"), (2, "b"), (2, "c") }.ToAsyncEnumerable();
        var second = new[] { (2, "x"), (3, "y"), (2, "z") }.ToAsyncEnumerable();

        // Act
        var result = await first
            .FullJoin(
                second,
                f => f.Item1,
                s => s.Item1,
                (f, s) => $"{f.Item2 ?? "-"}{s.Item2 ?? "-"}"
            )
            .ToArrayAsync();

        var asyncResult = await first
            .FullJoin(
                second,
                (f, _) => new ValueTask<int>(f.Item1),
                (s, _) => new ValueTask<int>(s.Item1),
                (f, s, _) => new ValueTask<string>($"{f.Item2 ?? "-"}{s.Item2 ?? "-"}")
            )
            .ToArrayAsync();

        // Assert
        result.Should().Equal("a-", "bx", "bz", "cx", "cz", "-y");
        asyncResult.Should().Equal("a-", "bx", "bz", "cx", "cz", "-y");
    }

    [Fact]
    public async Task FullJoin_EmptyFirstArray_PreservesSecondOrder_Test()
    {
        // Arrange
        var first = AsyncEnumerable.Empty<int>();
        var second = new[] { 1, 2, 1 }.ToAsyncEnumerable();
        var secondKeySelectorCalls = 0;

        // Act
        var result = await first
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
            .ToArrayAsync();

        // Assert
        result.Should().Equal("-1", "-2", "-1");
        secondKeySelectorCalls.Should().Be(0);
    }

    [Fact]
    public async Task FullJoin_NullKeys_DoNotMatch_Test()
    {
        // Arrange
        var first = new[] { ("a", null), ("b", "k") }.ToAsyncEnumerable();
        var second = new[] { ("x", null), ("y", "k") }.ToAsyncEnumerable();

        // Act
        var result = await first
            .FullJoin(
                second,
                f => f.Item2,
                s => s.Item2,
                (f, s) => $"{f.Item1 ?? "-"}{s.Item1 ?? "-"}"
            )
            .ToArrayAsync();

        // Assert
        result.Should().Equal("a-", "by", "-x");
    }
}
