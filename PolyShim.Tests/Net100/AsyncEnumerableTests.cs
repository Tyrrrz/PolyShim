using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Xunit;

namespace PolyShim.Tests.Net100;

public class AsyncEnumerableTests
{
    [Fact]
    public async Task Range_Empty_Repeat_Test()
    {
        (await AsyncEnumerable.Range(3, 3).ToListAsync()).Should().Equal(3, 4, 5);
        (await AsyncEnumerable.Empty<int>().ToListAsync()).Should().BeEmpty();
        (await AsyncEnumerable.Repeat("a", 2).ToListAsync()).Should().Equal("a", "a");
    }

    [Fact]
    public async Task Where_Select_Test()
    {
        var source = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();

        (await source.Where(x => x % 2 == 1).Select(x => x * 10).ToListAsync())
            .Should()
            .Equal(10, 30, 50);
        (await source.Where((x, i) => i < 2).ToListAsync()).Should().Equal(1, 2);
        (await source.Select((x, i) => x + i).ToListAsync()).Should().Equal(1, 3, 5, 7, 9);
        (await source.WhereAwait((x, _) => new ValueTask<bool>(x > 3)).ToListAsync())
            .Should()
            .Equal(4, 5);
        (await source.SelectAwait((x, _) => new ValueTask<int>(x + 1)).ToListAsync())
            .Should()
            .Equal(2, 3, 4, 5, 6);
    }

    [Fact]
    public async Task SkipTake_Test()
    {
        var source = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();

        (await source.Skip(2).Take(2).ToListAsync()).Should().Equal(3, 4);
        (await source.SkipWhile(x => x < 3).ToListAsync()).Should().Equal(3, 4, 5);
        (await source.TakeWhile(x => x < 3).ToListAsync()).Should().Equal(1, 2);
    }

    [Fact]
    public async Task Combine_Test()
    {
        var source = new[] { 1, 2, 2 }.ToAsyncEnumerable();

        (await source.Append(9).Prepend(0).ToListAsync()).Should().Equal(0, 1, 2, 2, 9);
        (await source.Concat(source).CountAsync()).Should().Be(6);
        (await source.Distinct().ToListAsync()).Should().Equal(1, 2);
        (await source.Reverse().ToListAsync()).Should().Equal(2, 2, 1);
        (await source.SelectMany(x => AsyncEnumerable.Repeat(x, x)).ToListAsync())
            .Should()
            .Equal(1, 2, 2, 2, 2);
        (await source.DefaultIfEmpty(5).ToListAsync()).Should().Equal(1, 2, 2);
        (await AsyncEnumerable.Empty<int>().DefaultIfEmpty(5).ToListAsync()).Should().Equal(5);
    }

    [Fact]
    public async Task Terminals_Test()
    {
        var source = new[] { 3, 1, 2 }.ToAsyncEnumerable();
        var empty = AsyncEnumerable.Empty<int>();

        (await source.AnyAsync()).Should().BeTrue();
        (await empty.AnyAsync()).Should().BeFalse();
        (await source.AnyAsync(x => x > 2)).Should().BeTrue();
        (await source.AllAsync(x => x > 2)).Should().BeFalse();
        (await source.CountAsync(x => x > 1)).Should().Be(2);
        (await source.LongCountAsync()).Should().Be(3);
        (await source.ContainsAsync(2)).Should().BeTrue();
        (await source.FirstAsync()).Should().Be(3);
        (await source.LastAsync()).Should().Be(2);
        (await source.ElementAtAsync(1)).Should().Be(1);
        (await source.ElementAtOrDefaultAsync(5)).Should().Be(0);
        (await source.MaxAsync()).Should().Be(3);
        (await source.MinAsync()).Should().Be(1);
        (await source.AggregateAsync((a, b) => a + b)).Should().Be(6);
        (await source.AggregateAsync(10, (a, b) => a + b)).Should().Be(16);
        (await source.SequenceEqualAsync(new[] { 3, 1, 2 }.ToAsyncEnumerable())).Should().BeTrue();
        (await source.ToArrayAsync()).Should().Equal(3, 1, 2);
        (await source.ToHashSetAsync()).Should().BeEquivalentTo(new[] { 1, 2, 3 });
        (await source.ToDictionaryAsync(x => x * 2)).Should().ContainKey(6);
        (await empty.FirstOrDefaultAsync()).Should().Be(0);
        (await empty.SingleOrDefaultAsync()).Should().Be(0);
        (await new[] { 7 }.ToAsyncEnumerable().SingleAsync()).Should().Be(7);

        await FluentActions
            .Awaiting(async () => await empty.FirstAsync())
            .Should()
            .ThrowAsync<InvalidOperationException>();
        await FluentActions
            .Awaiting(async () => await source.SingleAsync())
            .Should()
            .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Cast_OfType_Test()
    {
        var source = new object?[] { 1, "a", 2 }.ToAsyncEnumerable();

        (await source.OfType<int>().ToListAsync()).Should().Equal(1, 2);
        await FluentActions
            .Awaiting(async () => await source.Cast<int>().ToListAsync())
            .Should()
            .ThrowAsync<InvalidCastException>();
    }

    [Fact]
    public async Task OrderBy_GroupBy_Test()
    {
        var source = new[] { "bb", "a", "ccc", "aa", "b" }.ToAsyncEnumerable();

        (await source.OrderBy(x => x.Length).ThenByDescending(x => x).ToListAsync())
            .Should()
            .Equal("b", "a", "bb", "aa", "ccc");
        (await source.OrderByDescending(x => x.Length).ToListAsync())
            .Should()
            .Equal("ccc", "bb", "aa", "a", "b");

        var groups = await source.GroupBy(x => x.Length).ToListAsync();
        groups.Select(g => g.Key).Should().Equal(2, 1, 3);
        groups[1].Should().Equal("a", "b");
    }

    [Fact]
    public async Task Join_Zip_SetOps_Test()
    {
        var a = new[] { 1, 2, 3 }.ToAsyncEnumerable();
        var b = new[] { 3, 4 }.ToAsyncEnumerable();

        (await a.Join(b, x => x, x => x, (x, y) => x + y).ToListAsync()).Should().Equal(6);
        (await a.Zip(b).ToListAsync()).Should().Equal((1, 3), (2, 4));
        (await a.Zip(b, (x, y) => x * y).ToListAsync()).Should().Equal(3, 8);
        (await a.Except(b).ToListAsync()).Should().Equal(1, 2);
        (await a.Union(b).ToListAsync()).Should().Equal(1, 2, 3, 4);
    }

    [Fact]
    public async Task Sum_Test()
    {
        (await new[] { 1, 2, 3 }.ToAsyncEnumerable().SumAsync()).Should().Be(6);
        (
            await new double?[] { 1.5, null, 2 }
                .ToAsyncEnumerable()
                .SumAsync()
        ).Should().Be(3.5);
        (await new[] { "a", "bb" }.ToAsyncEnumerable().SumAsync(x => (long)x.Length))
            .Should()
            .Be(3);
    }
}
