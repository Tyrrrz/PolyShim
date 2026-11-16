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
    public async Task DefaultIfEmpty_Test()
    {
        // Act & assert
        (await new[] { 1, 2 }.ToAsyncEnumerable().DefaultIfEmpty(5).ToArrayAsync())
            .Should()
            .Equal(1, 2);

        (await AsyncEnumerable.Empty<int>().DefaultIfEmpty(5).ToArrayAsync()).Should().Equal(5);
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
    public async Task MaxAsync_Test()
    {
        // Act & assert
        (await new[] { 3, 1, 2 }.ToAsyncEnumerable().MaxAsync())
            .Should()
            .Be(3);
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
