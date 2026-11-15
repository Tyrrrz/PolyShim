using System;
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
        var first = new[] { 1, 2, 3 }.ToAsyncEnumerable();
        var second = new[] { 3, 4 }.ToAsyncEnumerable();

        var result = await first
            .FullJoin(second, x => x, x => x, (a, b) => $"{a}-{b}")
            .ToListAsync();

        result.Should().Equal("1-0", "2-0", "3-3", "0-4");

        var tupleResult = await first.FullJoin(second, x => x, x => x).ToListAsync();
        tupleResult.Should().Equal((1, 0), (2, 0), (3, 3), (0, 4));

        var asyncResult = await first
            .FullJoin(
                second,
                (x, _) => new ValueTask<int>(x),
                (x, _) => new ValueTask<int>(x),
                (a, b, _) => new ValueTask<string>($"{a}-{b}")
            )
            .ToListAsync();
        asyncResult.Should().Equal("1-0", "2-0", "3-3", "0-4");

        var asyncTupleResult = await first
            .FullJoin(second, (x, _) => new ValueTask<int>(x), (x, _) => new ValueTask<int>(x))
            .ToListAsync();
        asyncTupleResult.Should().Equal((1, 0), (2, 0), (3, 3), (0, 4));

        var secondWithDuplicates = new[] { 3, 4, 3 }.ToAsyncEnumerable();
        Func<int, int> throwingKeySelector = _ =>
            throw new InvalidOperationException("The key selector should not run.");
        Func<int, int, string> resultSelector = (firstItem, secondItem) =>
            $"{firstItem}-{secondItem}";
        var emptyFirstResult = await AsyncEnumerable
            .Empty<int>()
            .FullJoin(
                secondWithDuplicates,
                throwingKeySelector,
                throwingKeySelector,
                resultSelector
            )
            .ToListAsync();
        emptyFirstResult.Should().Equal("0-3", "0-4", "0-3");

        var firstForEmptySecond = new[] { 1, 2, 3 }.ToAsyncEnumerable();
        var emptySecondResult = await firstForEmptySecond
            .FullJoin(
                AsyncEnumerable.Empty<int>(),
                throwingKeySelector,
                throwingKeySelector,
                resultSelector
            )
            .ToListAsync();
        emptySecondResult.Should().Equal("1-0", "2-0", "3-0");
    }
}
