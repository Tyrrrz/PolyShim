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
    public async Task Empty_Test()
    {
        // Act & assert
        (await AsyncEnumerable.Empty<int>().ToArrayAsync())
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Range_Test()
    {
        // Act & assert
        (await AsyncEnumerable.Range(3, 3).ToArrayAsync())
            .Should()
            .Equal(3, 4, 5);
    }

    [Fact]
    public async Task Repeat_Test()
    {
        // Act & assert
        (await AsyncEnumerable.Repeat("a", 2).ToArrayAsync())
            .Should()
            .Equal("a", "a");
    }

    [Fact]
    public async Task ToAsyncEnumerable_Test()
    {
        // Act & assert
        (await new[] { 1, 2, 3 }.ToAsyncEnumerable().ToArrayAsync())
            .Should()
            .Equal(1, 2, 3);
    }

    [Fact]
    public async Task AggregateAsync_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3 }.ToAsyncEnumerable();

        // Act & assert
        (await source.AggregateAsync((a, b) => a + b))
            .Should()
            .Be(6);
        (await source.AggregateAsync(10, (a, b) => a + b)).Should().Be(16);

        // The first element is the seed when none is provided
        (await source.AggregateAsync((a, b) => a - b))
            .Should()
            .Be(-4);
        (await source.AggregateAsync((a, b, _) => new ValueTask<int>(a - b))).Should().Be(-4);

        var act = async () => await AsyncEnumerable.Empty<int>().AggregateAsync((a, b) => a + b);
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AggregateBy_Test()
    {
        // Arrange
        var source = new[] { "a", "bb", "c", "dd", "eee" }.ToAsyncEnumerable();

        // Act & assert
        (
            await source
                .AggregateBy(x => x.Length, 0, (acc, x) => acc + x.Length)
                .Select(p => (p.Key, p.Value))
                .ToArrayAsync()
        )
            .Should()
            .Equal((1, 2), (2, 4), (3, 3));

        (
            await source
                .AggregateBy(x => x.Length, k => k * 10, (acc, x) => acc + x.Length)
                .Select(p => (p.Key, p.Value))
                .ToArrayAsync()
        )
            .Should()
            .Equal((1, 12), (2, 24), (3, 33));

        (
            await source
                .AggregateBy(
                    (x, _) => new ValueTask<int>(x.Length),
                    0,
                    (acc, x, _) => new ValueTask<int>(acc + x.Length)
                )
                .Select(p => (p.Key, p.Value))
                .ToArrayAsync()
        )
            .Should()
            .Equal((1, 2), (2, 4), (3, 3));

        (
            await source
                .AggregateBy(
                    (x, _) => new ValueTask<int>(x.Length),
                    (k, _) => new ValueTask<int>(k * 10),
                    (acc, x, _) => new ValueTask<int>(acc + x.Length)
                )
                .Select(p => (p.Key, p.Value))
                .ToArrayAsync()
        )
            .Should()
            .Equal((1, 12), (2, 24), (3, 33));

        (
            await AsyncEnumerable
                .Empty<string>()
                .AggregateBy(x => x.Length, 0, (acc, x) => acc + x.Length)
                .ToArrayAsync()
        )
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task AllAsync_Test()
    {
        // Arrange
        var source = new[] { 3, 1, 2 }.ToAsyncEnumerable();

        // Act & assert
        (await source.AllAsync(x => x > 0))
            .Should()
            .BeTrue();
        (await source.AllAsync(x => x > 2)).Should().BeFalse();
        (await source.AllAsync((x, _) => new ValueTask<bool>(x > 0))).Should().BeTrue();
    }

    [Fact]
    public async Task AnyAsync_Test()
    {
        // Arrange
        var source = new[] { 3, 1, 2 }.ToAsyncEnumerable();

        // Act & assert
        (await source.AnyAsync())
            .Should()
            .BeTrue();
        (await source.AnyAsync(x => x > 2)).Should().BeTrue();
        (await source.AnyAsync((x, _) => new ValueTask<bool>(x > 2))).Should().BeTrue();
        (await AsyncEnumerable.Empty<int>().AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task Append_Test()
    {
        // Act & assert
        (await new[] { 1, 2 }.ToAsyncEnumerable().Append(3).ToArrayAsync())
            .Should()
            .Equal(1, 2, 3);
    }

    [Fact]
    public async Task Chunk_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();

        // Act
        var chunks = await source.Chunk(2).ToArrayAsync();

        // Assert
        chunks.Should().HaveCount(3);
        chunks[0].Should().Equal(1, 2);
        chunks[1].Should().Equal(3, 4);
        chunks[2].Should().Equal(5);

        (await source.Chunk(5).ToArrayAsync())
            .Should()
            .ContainSingle()
            .Which.Should()
            .Equal(1, 2, 3, 4, 5);
        (await AsyncEnumerable.Empty<int>().Chunk(2).ToArrayAsync()).Should().BeEmpty();

        var act = () => source.Chunk(0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public async Task Concat_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 2 }.ToAsyncEnumerable();

        // Act & assert
        (await source.Concat(source).ToArrayAsync())
            .Should()
            .Equal(1, 2, 2, 1, 2, 2);
    }

    [Fact]
    public async Task ContainsAsync_Test()
    {
        // Act & assert
        (await new[] { 1, 2, 3 }.ToAsyncEnumerable().ContainsAsync(2))
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task CountAsync_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3 }.ToAsyncEnumerable();

        // Act & assert
        (await source.CountAsync())
            .Should()
            .Be(3);
        (await source.CountAsync(x => x > 1)).Should().Be(2);
        (await source.CountAsync((x, _) => new ValueTask<bool>(x > 1))).Should().Be(2);
    }

    [Fact]
    public async Task CountBy_Test()
    {
        // Arrange
        var source = new[] { "a", "bb", "c", "dd", "eee" }.ToAsyncEnumerable();

        // Act & assert
        (await source.CountBy(x => x.Length).Select(p => (p.Key, p.Value)).ToArrayAsync())
            .Should()
            .Equal((1, 2), (2, 2), (3, 1));

        (
            await source
                .CountBy((x, _) => new ValueTask<int>(x.Length))
                .Select(p => (p.Key, p.Value))
                .ToArrayAsync()
        )
            .Should()
            .Equal((1, 2), (2, 2), (3, 1));

        (await AsyncEnumerable.Empty<string>().CountBy(x => x.Length).ToArrayAsync())
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task DefaultIfEmpty_Test()
    {
        // Act & assert
        (await new[] { 1, 2 }.ToAsyncEnumerable().DefaultIfEmpty(5).ToArrayAsync())
            .Should()
            .Equal(1, 2);

        (await AsyncEnumerable.Empty<int>().DefaultIfEmpty(5).ToArrayAsync()).Should().Equal(5);
    }

    [Fact]
    public async Task DistinctBy_Test()
    {
        // Arrange
        var source = new[] { "a", "bb", "c", "dd" }.ToAsyncEnumerable();

        // Act & assert
        (await source.DistinctBy(x => x.Length).ToArrayAsync())
            .Should()
            .Equal("a", "bb");
        (await source.DistinctBy((x, _) => new ValueTask<int>(x.Length)).ToArrayAsync())
            .Should()
            .Equal("a", "bb");
    }

    [Fact]
    public async Task Distinct_Test()
    {
        // Act & assert
        (await new[] { 1, 2, 2 }.ToAsyncEnumerable().Distinct().ToArrayAsync())
            .Should()
            .Equal(1, 2);
    }

    [Fact]
    public async Task ElementAtAsync_Test()
    {
        // Act & assert
        (await new[] { 3, 1, 2 }.ToAsyncEnumerable().ElementAtAsync(1))
            .Should()
            .Be(1);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_Test()
    {
        // Act & assert
        (await new[] { 3, 1, 2 }.ToAsyncEnumerable().ElementAtOrDefaultAsync(5))
            .Should()
            .Be(0);
    }

    [Fact]
    public async Task ExceptBy_Test()
    {
        // Arrange
        var source = new[] { "a", "bb", "ccc" }.ToAsyncEnumerable();
        var second = new[] { 2 }.ToAsyncEnumerable();

        // Act & assert
        (await source.ExceptBy(second, x => x.Length).ToArrayAsync())
            .Should()
            .Equal("a", "ccc");
        (await source.ExceptBy(second, (x, _) => new ValueTask<int>(x.Length)).ToArrayAsync())
            .Should()
            .Equal("a", "ccc");
    }

    [Fact]
    public async Task Except_Test()
    {
        // Act & assert
        (
            await new[] { 1, 2, 3 }
                .ToAsyncEnumerable()
                .Except(new[] { 3, 4 }.ToAsyncEnumerable())
                .ToArrayAsync()
        )
            .Should()
            .Equal(1, 2);
    }

    [Fact]
    public async Task FirstAsync_Test()
    {
        // Arrange
        var source = new[] { 3, 1, 2 }.ToAsyncEnumerable();

        // Act & assert
        (await source.FirstAsync())
            .Should()
            .Be(3);
        (await source.FirstAsync(x => x == 1)).Should().Be(1);

        var act = async () => await source.FirstAsync(x => x == 5);
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task FirstOrDefaultAsync_Test()
    {
        // Act & assert
        (await AsyncEnumerable.Empty<int>().FirstOrDefaultAsync())
            .Should()
            .Be(0);
    }

    [Fact]
    public async Task GroupBy_Test()
    {
        // Act & assert
        var groups = await new[] { "bb", "a", "ccc", "aa", "b" }
            .ToAsyncEnumerable()
            .GroupBy(x => x.Length)
            .ToArrayAsync();

        groups.Select(g => g.Key).Should().Equal(2, 1, 3);
        groups[1].Should().Equal("a", "b");
    }

    [Fact]
    public async Task Index_Test()
    {
        // Act & assert
        (await new[] { "a", "b", "c" }.ToAsyncEnumerable().Index().ToArrayAsync())
            .Should()
            .Equal((0, "a"), (1, "b"), (2, "c"));
        (await AsyncEnumerable.Empty<int>().Index().ToArrayAsync()).Should().BeEmpty();
    }

    [Fact]
    public async Task IntersectBy_Test()
    {
        // Arrange
        var source = new[] { "a", "bb", "ccc", "dd" }.ToAsyncEnumerable();
        var second = new[] { 2, 3 }.ToAsyncEnumerable();

        // Act & assert
        (await source.IntersectBy(second, x => x.Length).ToArrayAsync())
            .Should()
            .Equal("bb", "ccc");
        (await source.IntersectBy(second, (x, _) => new ValueTask<int>(x.Length)).ToArrayAsync())
            .Should()
            .Equal("bb", "ccc");
    }

    [Fact]
    public async Task Intersect_Test()
    {
        // Act & assert
        (
            await new[] { 1, 2, 2, 3 }
                .ToAsyncEnumerable()
                .Intersect(new[] { 2, 3, 4 }.ToAsyncEnumerable())
                .ToArrayAsync()
        )
            .Should()
            .Equal(2, 3);
    }

    [Fact]
    public async Task Join_Test()
    {
        // Arrange
        var first = new[] { 1, 2, 3 }.ToAsyncEnumerable();
        var second = new[] { 3, 4 }.ToAsyncEnumerable();

        // Act & assert
        (await first.Join(second, x => x, x => x, (x, y) => x + y).ToArrayAsync())
            .Should()
            .Equal(6);
    }

    [Fact]
    public async Task LastAsync_Test()
    {
        // Act & assert
        (await new[] { 3, 1, 2 }.ToAsyncEnumerable().LastAsync())
            .Should()
            .Be(2);
    }

    [Fact]
    public async Task LastOrDefaultAsync_Test()
    {
        // Act & assert
        (await AsyncEnumerable.Empty<int>().LastOrDefaultAsync())
            .Should()
            .Be(0);
    }

    [Fact]
    public async Task LongCountAsync_Test()
    {
        // Act & assert
        (await new[] { 1, 2, 3 }.ToAsyncEnumerable().LongCountAsync())
            .Should()
            .Be(3);
    }

    [Fact]
    public async Task MaxByAsync_Test()
    {
        // Arrange
        var source = new[] { "bb", "a", "ccc", "dd" }.ToAsyncEnumerable();

        // Act & assert
        (await source.MaxByAsync(x => x.Length))
            .Should()
            .Be("ccc");
        (await source.MaxByAsync((x, _) => new ValueTask<int>(x.Length))).Should().Be("ccc");
        (await new[] { "a", "b" }.ToAsyncEnumerable().MaxByAsync(x => x.Length)).Should().Be("a");
        (await AsyncEnumerable.Empty<string>().MaxByAsync(x => x.Length)).Should().BeNull();

        var act = async () => await AsyncEnumerable.Empty<int>().MaxByAsync(x => x);
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MaxAsync_Test()
    {
        // Act & assert
        (await new[] { 3, 1, 2 }.ToAsyncEnumerable().MaxAsync())
            .Should()
            .Be(3);
    }

    [Fact]
    public async Task MinByAsync_Test()
    {
        // Arrange
        var source = new[] { "bb", "a", "ccc", "b" }.ToAsyncEnumerable();

        // Act & assert
        (await source.MinByAsync(x => x.Length))
            .Should()
            .Be("a");
        (await source.MinByAsync((x, _) => new ValueTask<int>(x.Length))).Should().Be("a");
        (await AsyncEnumerable.Empty<string>().MinByAsync(x => x.Length)).Should().BeNull();

        var act = async () => await AsyncEnumerable.Empty<int>().MinByAsync(x => x);
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MinAsync_Test()
    {
        // Act & assert
        (await new[] { 3, 1, 2 }.ToAsyncEnumerable().MinAsync())
            .Should()
            .Be(1);
    }

    [Fact]
    public async Task OrderBy_Test()
    {
        // Arrange
        var source = new[] { "bb", "a", "ccc", "aa", "b" }.ToAsyncEnumerable();

        // Act & assert
        (await source.OrderBy(x => x.Length).ToArrayAsync())
            .Should()
            .Equal("a", "b", "bb", "aa", "ccc");
    }

    [Fact]
    public async Task Order_Test()
    {
        // Act & assert
        (await new[] { 3, 1, 2 }.ToAsyncEnumerable().Order().ToArrayAsync())
            .Should()
            .Equal(1, 2, 3);
    }

    [Fact]
    public async Task OrderByDescending_Test()
    {
        // Arrange
        var source = new[] { "bb", "a", "ccc", "aa", "b" }.ToAsyncEnumerable();

        // Act & assert
        (await source.OrderByDescending(x => x.Length).ToArrayAsync())
            .Should()
            .Equal("ccc", "bb", "aa", "a", "b");
    }

    [Fact]
    public async Task OrderDescending_Test()
    {
        // Act & assert
        (await new[] { 3, 1, 2 }.ToAsyncEnumerable().OrderDescending().ToArrayAsync())
            .Should()
            .Equal(3, 2, 1);
    }

    [Fact]
    public async Task Prepend_Test()
    {
        // Act & assert
        (await new[] { 1, 2 }.ToAsyncEnumerable().Prepend(0).ToArrayAsync())
            .Should()
            .Equal(0, 1, 2);
    }

    [Fact]
    public async Task Reverse_Test()
    {
        // Act & assert
        (await new[] { 1, 2, 3 }.ToAsyncEnumerable().Reverse().ToArrayAsync())
            .Should()
            .Equal(3, 2, 1);
    }

    [Fact]
    public async Task Select_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();

        // Act & assert
        (await source.Select(x => x * 10).ToArrayAsync())
            .Should()
            .Equal(10, 20, 30, 40, 50);
        (await source.Select((x, i) => x + i).ToArrayAsync()).Should().Equal(1, 3, 5, 7, 9);
        (
            await source
                .Select((int x, CancellationToken _) => new ValueTask<int>(x + 1))
                .ToArrayAsync()
        )
            .Should()
            .Equal(2, 3, 4, 5, 6);
    }

    [Fact]
    public async Task SelectMany_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3 }.ToAsyncEnumerable();

        // Act & assert
        (await source.SelectMany(x => AsyncEnumerable.Repeat(x, x)).ToArrayAsync())
            .Should()
            .Equal(1, 2, 2, 3, 3, 3);
    }

    [Fact]
    public async Task SequenceEqualAsync_Test()
    {
        // Act & assert
        (
            await new[] { 3, 1, 2 }
                .ToAsyncEnumerable()
                .SequenceEqualAsync(new[] { 3, 1, 2 }.ToAsyncEnumerable())
        )
            .Should()
            .BeTrue();

        (
            await new[] { 3, 1, 2 }
                .ToAsyncEnumerable()
                .SequenceEqualAsync(new[] { 1, 2, 3 }.ToAsyncEnumerable())
        )
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task Shuffle_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();

        // Act & assert
        (await source.Shuffle().ToArrayAsync())
            .Should()
            .BeEquivalentTo([1, 2, 3, 4, 5]);
        (await AsyncEnumerable.Empty<int>().Shuffle().ToArrayAsync()).Should().BeEmpty();
    }

    [Fact]
    public async Task SingleAsync_Test()
    {
        // Act & assert
        (await new[] { 7 }.ToAsyncEnumerable().SingleAsync())
            .Should()
            .Be(7);

        var act = async () => await new[] { 3, 1, 2 }.ToAsyncEnumerable().SingleAsync();
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task SingleOrDefaultAsync_Test()
    {
        // Act & assert
        (await AsyncEnumerable.Empty<int>().SingleOrDefaultAsync())
            .Should()
            .Be(0);
    }

    [Fact]
    public async Task Skip_Test()
    {
        // Act & assert
        (await new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Skip(2).ToArrayAsync())
            .Should()
            .Equal(3, 4, 5);
    }

    [Fact]
    public async Task SkipLast_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();

        // Act & assert
        (await source.SkipLast(2).ToArrayAsync())
            .Should()
            .Equal(1, 2, 3);
        (await source.SkipLast(0).ToArrayAsync()).Should().Equal(1, 2, 3, 4, 5);
        (await source.SkipLast(10).ToArrayAsync()).Should().BeEmpty();
    }

    [Fact]
    public async Task SkipWhile_Test()
    {
        // Act & assert
        (await new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().SkipWhile(x => x < 3).ToArrayAsync())
            .Should()
            .Equal(3, 4, 5);
    }

    [Fact]
    public async Task Take_Test()
    {
        // Act & assert
        (await new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().Take(2).ToArrayAsync())
            .Should()
            .Equal(1, 2);
    }

    [Fact]
    public async Task TakeLast_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();

        // Act & assert
        (await source.TakeLast(2).ToArrayAsync())
            .Should()
            .Equal(4, 5);
        (await source.TakeLast(0).ToArrayAsync()).Should().BeEmpty();
        (await source.TakeLast(10).ToArrayAsync()).Should().Equal(1, 2, 3, 4, 5);
    }

    [Fact]
    public async Task TakeWhile_Test()
    {
        // Act & assert
        (await new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable().TakeWhile(x => x < 3).ToArrayAsync())
            .Should()
            .Equal(1, 2);
    }

    [Fact]
    public async Task ToArrayAsync_Test()
    {
        // Act & assert
        (await new[] { 3, 1, 2 }.ToAsyncEnumerable().ToArrayAsync())
            .Should()
            .Equal(3, 1, 2);
    }

    [Fact]
    public async Task ToDictionaryAsync_Test()
    {
        // Act & assert
        (await new[] { 3, 1, 2 }.ToAsyncEnumerable().ToDictionaryAsync(x => x * 2))
            .Should()
            .ContainKey(6);
    }

    [Fact]
    public async Task ToHashSetAsync_Test()
    {
        // Act & assert
        (await new[] { 1, 2, 2 }.ToAsyncEnumerable().ToHashSetAsync())
            .Should()
            .BeEquivalentTo([1, 2]);
    }

    [Fact]
    public async Task ToListAsync_Test()
    {
        // Act & assert
        (await new[] { 3, 1, 2 }.ToAsyncEnumerable().ToArrayAsync())
            .Should()
            .Equal(3, 1, 2);
    }

    [Fact]
    public async Task UnionBy_Test()
    {
        // Arrange
        var source = new[] { "a", "bb" }.ToAsyncEnumerable();
        var second = new[] { "c", "ddd" }.ToAsyncEnumerable();

        // Act & assert
        (await source.UnionBy(second, x => x.Length).ToArrayAsync())
            .Should()
            .Equal("a", "bb", "ddd");
        (await source.UnionBy(second, (x, _) => new ValueTask<int>(x.Length)).ToArrayAsync())
            .Should()
            .Equal("a", "bb", "ddd");
    }

    [Fact]
    public async Task Union_Test()
    {
        // Act & assert
        (
            await new[] { 1, 2, 3 }
                .ToAsyncEnumerable()
                .Union(new[] { 3, 4 }.ToAsyncEnumerable())
                .ToArrayAsync()
        )
            .Should()
            .Equal(1, 2, 3, 4);
    }

    [Fact]
    public async Task Where_Test()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 }.ToAsyncEnumerable();

        // Act & assert
        (await source.Where(x => x % 2 == 1).ToArrayAsync())
            .Should()
            .Equal(1, 3, 5);
        (await source.Where((x, i) => i < 2).ToArrayAsync()).Should().Equal(1, 2);
        (await source.Where((x, _) => new ValueTask<bool>(x > 3)).ToArrayAsync())
            .Should()
            .Equal(4, 5);
    }

    [Fact]
    public async Task Zip_Test()
    {
        // Arrange
        var first = new[] { 1, 2, 3 }.ToAsyncEnumerable();
        var second = new[] { 3, 4 }.ToAsyncEnumerable();

        // Act & assert
        (await first.Zip(second).ToArrayAsync())
            .Should()
            .Equal((1, 3), (2, 4));
        (await first.Zip(second, (x, y) => x * y).ToArrayAsync()).Should().Equal(3, 8);
    }

    [Fact]
    public async Task AverageAsync_Test()
    {
        // Act & assert
        (await new[] { 1, 2, 4 }.ToAsyncEnumerable().AverageAsync())
            .Should()
            .BeApproximately(7.0 / 3, 0.0001);
        (
            await new int?[] { 1, null, 3 }
                .ToAsyncEnumerable()
                .AverageAsync()
        ).Should().Be(2.0);
        (await new[] { 1L, 2L }.ToAsyncEnumerable().AverageAsync()).Should().Be(1.5);
        (
            await new long?[] { 1L, null, 2L }
                .ToAsyncEnumerable()
                .AverageAsync()
        ).Should().Be(1.5);
        (await new[] { 1.5, 2.5 }.ToAsyncEnumerable().AverageAsync()).Should().Be(2.0);
        (
            await new double?[] { 1.5, null, 2.5 }
                .ToAsyncEnumerable()
                .AverageAsync()
        ).Should().Be(2.0);
        (await new[] { 1f, 2f }.ToAsyncEnumerable().AverageAsync()).Should().Be(1.5f);
        (
            await new float?[] { 1f, null, 2f }
                .ToAsyncEnumerable()
                .AverageAsync()
        ).Should().Be(1.5f);
        (await new[] { 1m, 2m }.ToAsyncEnumerable().AverageAsync()).Should().Be(1.5m);
        (
            await new decimal?[] { 1m, null, 2m }
                .ToAsyncEnumerable()
                .AverageAsync()
        ).Should().Be(1.5m);

        (await AsyncEnumerable.Empty<int?>().AverageAsync()).Should().BeNull();
        (
            await new int?[] { null, null }
                .ToAsyncEnumerable()
                .AverageAsync()
        ).Should().BeNull();

        var act = async () => await AsyncEnumerable.Empty<int>().AverageAsync();
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task SumAsync_Test()
    {
        // Act & assert
        (await new[] { 1, 2, 3 }.ToAsyncEnumerable().SumAsync())
            .Should()
            .Be(6);
        (
            await new double?[] { 1.5, null, 2 }
                .ToAsyncEnumerable()
                .SumAsync()
        ).Should().Be(3.5);

        (await new[] { 16777216f, 1f, -16777216f }.ToAsyncEnumerable().SumAsync()).Should().Be(1f);
        (
            await new float?[] { 16777216f, null, 1f, -16777216f }
                .ToAsyncEnumerable()
                .SumAsync()
        ).Should().Be(1f);

        (await new[] { "a", "bb" }.ToAsyncEnumerable().Select(x => (long)x.Length).SumAsync())
            .Should()
            .Be(3);
    }

    [Fact]
    public async Task ThenBy_Test()
    {
        // Arrange
        var source = new[] { "bb", "a", "ccc", "aa", "b" }.ToAsyncEnumerable();

        // Act & assert
        (await source.OrderBy(x => x.Length).ThenBy(x => x).ToArrayAsync())
            .Should()
            .Equal("a", "b", "aa", "bb", "ccc");

        (
            await source
                .OrderBy(x => x.Length)
                .CreateOrderedAsyncEnumerable((x, _) => new ValueTask<char>(x[0]), null, false)
                .ToArrayAsync()
        )
            .Should()
            .Equal("a", "b", "aa", "bb", "ccc");
    }

    [Fact]
    public async Task ThenByDescending_Test()
    {
        // Arrange
        var source = new[] { "bb", "a", "ccc", "aa", "b" }.ToAsyncEnumerable();

        // Act & assert
        (await source.OrderBy(x => x.Length).ThenByDescending(x => x).ToArrayAsync())
            .Should()
            .Equal("b", "a", "bb", "aa", "ccc");
    }
}
