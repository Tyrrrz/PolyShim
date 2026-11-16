#if (NETCOREAPP && !NET10_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#if FEATURE_TASK && !FEATURE_ASYNC_LINQ
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
// https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable
internal static class AsyncEnumerable
{
    // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.empty
    public static IAsyncEnumerable<T> Empty<T>() => Enumerable.Empty<T>().ToAsyncEnumerable();

    // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.range
    public static IAsyncEnumerable<int> Range(int start, int count) =>
        Enumerable.Range(start, count).ToAsyncEnumerable();

    // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.repeat
    public static IAsyncEnumerable<T> Repeat<T>(T element, int count) =>
        Enumerable.Repeat(element, count).ToAsyncEnumerable();

    extension<T>(IEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.toasyncenumerable
        public IAsyncEnumerable<T> ToAsyncEnumerable()
        {
            ArgumentNullException.ThrowIfNull(source);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                foreach (var item in source)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    yield return item;
                }
            }

            return Iterate();
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.aggregateasync?system-linq-asyncenumerable-aggregateasync-3(system-collections-generic-iasyncenumerable((-0))-1-system-func((-1-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-func((-1-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-threading-cancellationtoken)
        public async ValueTask<TResult> AggregateAsync<TAccumulate, TResult>(
            TAccumulate seed,
            Func<TAccumulate, T, CancellationToken, ValueTask<TAccumulate>> func,
            Func<TAccumulate, CancellationToken, ValueTask<TResult>> resultSelector,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(func);
            ArgumentNullException.ThrowIfNull(resultSelector);

            var result = seed;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                result = await func(result, item, cancellationToken).ConfigureAwait(false);
            }

            return await resultSelector(result, cancellationToken).ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.aggregateasync?system-linq-asyncenumerable-aggregateasync-3(system-collections-generic-iasyncenumerable((-0))-1-system-func((-1-0-1))-system-func((-1-2))-system-threading-cancellationtoken)
        public async ValueTask<TResult> AggregateAsync<TAccumulate, TResult>(
            TAccumulate seed,
            Func<TAccumulate, T, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .AggregateAsync(
                    seed,
                    (acc, item, _) => new ValueTask<TAccumulate>(func(acc, item)),
                    (acc, _) => new ValueTask<TResult>(resultSelector(acc)),
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.aggregateasync?system-linq-asyncenumerable-aggregateasync-2(system-collections-generic-iasyncenumerable((-0))-1-system-func((-1-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-threading-cancellationtoken)
        public async ValueTask<TAccumulate> AggregateAsync<TAccumulate>(
            TAccumulate seed,
            Func<TAccumulate, T, CancellationToken, ValueTask<TAccumulate>> func,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .AggregateAsync(
                    seed,
                    func,
                    (acc, _) => new ValueTask<TAccumulate>(acc),
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.aggregateasync?system-linq-asyncenumerable-aggregateasync-2(system-collections-generic-iasyncenumerable((-0))-1-system-func((-1-0-1))-system-threading-cancellationtoken)
        public async ValueTask<TAccumulate> AggregateAsync<TAccumulate>(
            TAccumulate seed,
            Func<TAccumulate, T, TAccumulate> func,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .AggregateAsync(
                    seed,
                    (acc, item, _) => new ValueTask<TAccumulate>(func(acc, item)),
                    (acc, _) => new ValueTask<TAccumulate>(acc),
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.aggregateasync?system-linq-asyncenumerable-aggregateasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-0))))-system-threading-cancellationtoken)
        public async ValueTask<T> AggregateAsync(
            Func<T, T, CancellationToken, ValueTask<T>> func,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .AggregateAsync(
                    default!,
                    func,
                    (acc, _) => new ValueTask<T>(acc),
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.aggregateasync?system-linq-asyncenumerable-aggregateasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-0-0))-system-threading-cancellationtoken)
        public async ValueTask<T> AggregateAsync(
            Func<T, T, T> accumulator,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .AggregateAsync(
                    (acc, item, _) => new ValueTask<T>(accumulator(acc, item)),
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.allasync?system-linq-asyncenumerable-allasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        public async ValueTask<bool> AllAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (!await predicate(item, cancellationToken).ConfigureAwait(false))
                    return false;
            }

            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.allasync?system-linq-asyncenumerable-allasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-system-threading-cancellationtoken)
        public async ValueTask<bool> AllAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .AllAsync((item, _) => new ValueTask<bool>(predicate(item)), cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.anyasync?system-linq-asyncenumerable-anyasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        public async ValueTask<bool> AnyAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (await predicate(item, cancellationToken).ConfigureAwait(false))
                    return true;
            }

            return false;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.anyasync?system-linq-asyncenumerable-anyasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        public async ValueTask<bool> AnyAsync(
            Func<T, CancellationToken, bool> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .AnyAsync((item, ct) => new ValueTask<bool>(predicate(item, ct)), cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.anyasync?system-linq-asyncenumerable-anyasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-system-threading-cancellationtoken)
        public async ValueTask<bool> AnyAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .AnyAsync((item, _) => predicate(item), cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.anyasync?system-linq-asyncenumerable-anyasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-cancellationtoken)
        public async ValueTask<bool> AnyAsync(CancellationToken cancellationToken = default) =>
            await source.AnyAsync(_ => true, cancellationToken).ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.append
        public IAsyncEnumerable<T> Append(T element)
        {
            ArgumentNullException.ThrowIfNull(source);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                    yield return item;

                yield return element;
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.concat
        public IAsyncEnumerable<T> Concat(IAsyncEnumerable<T> second)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(second);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    yield return item;
                }

                await foreach (
                    var item in second.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    yield return item;
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.containsasync
        public async ValueTask<bool> ContainsAsync(
            T value,
            IEqualityComparer<T>? comparer = null,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if ((comparer ?? EqualityComparer<T>.Default).Equals(item, value))
                    return true;
            }

            return false;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.countasync?system-linq-asyncenumerable-countasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        public async ValueTask<int> CountAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            var count = 0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (await predicate(item, cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.countasync?system-linq-asyncenumerable-countasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-system-threading-cancellationtoken)
        public async ValueTask<int> CountAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .CountAsync((item, _) => new ValueTask<bool>(predicate(item)), cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.countasync?system-linq-asyncenumerable-countasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-cancellationtoken)
        public async ValueTask<int> CountAsync(CancellationToken cancellationToken = default) =>
            await source.CountAsync(_ => true, cancellationToken).ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.defaultifempty?system-linq-asyncenumerable-defaultifempty-1(system-collections-generic-iasyncenumerable((-0))-0)
        public IAsyncEnumerable<T?> DefaultIfEmpty(T defaultValue)
        {
            ArgumentNullException.ThrowIfNull(source);

            async IAsyncEnumerable<T?> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var any = false;
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    any = true;
                    yield return item;
                }

                if (!any)
                    yield return defaultValue;
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.defaultifempty?system-linq-asyncenumerable-defaultifempty-1(system-collections-generic-iasyncenumerable((-0)))
        public IAsyncEnumerable<T?> DefaultIfEmpty() => source.DefaultIfEmpty(default!);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.distinct
        public IAsyncEnumerable<T> Distinct(IEqualityComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(source);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var seen = new HashSet<T>(comparer);
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (seen.Add(item))
                        yield return item;
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.elementatasync?system-linq-asyncenumerable-elementatasync-1(system-collections-generic-iasyncenumerable((-0))-system-int32-system-threading-cancellationtoken)
        public async ValueTask<T> ElementAtAsync(
            int index,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentOutOfRangeException.ThrowIfNegative(index);

            var i = 0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (i++ == index)
                    return item;
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.elementatasync?system-linq-asyncenumerable-elementatasync-1(system-collections-generic-iasyncenumerable((-0))-system-index-system-threading-cancellationtoken)
        public async ValueTask<T> ElementAtAsync(
            Index index,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);

            var items = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

            var i = index.IsFromEnd ? items.Count - index.Value : index.Value;
            if (i < 0 || i >= items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return items[i];
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.elementatordefaultasync?system-linq-asyncenumerable-elementatordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-int32-system-threading-cancellationtoken)
        public async ValueTask<T?> ElementAtOrDefaultAsync(
            int index,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);

            if (index < 0)
                return default;

            var i = 0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (i++ == index)
                    return item;
            }

            return default;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.elementatordefaultasync?system-linq-asyncenumerable-elementatordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-index-system-threading-cancellationtoken)
        public async ValueTask<T?> ElementAtOrDefaultAsync(
            Index index,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);

            var items = await source.ToListAsync(cancellationToken).ConfigureAwait(false);

            var i = index.IsFromEnd ? items.Count - index.Value : index.Value;
            if (i < 0 || i >= items.Count)
                return default;

            return items[i];
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.except
        public IAsyncEnumerable<T> Except(
            IAsyncEnumerable<T> second,
            IEqualityComparer<T>? comparer = null
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(second);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var seen = await second
                    .ToHashSetAsync(comparer, cancellationToken)
                    .ConfigureAwait(false);
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (seen.Add(item))
                        yield return item;
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstasync?system-linq-asyncenumerable-firstasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        public async ValueTask<T> FirstAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (await predicate(item, cancellationToken).ConfigureAwait(false))
                    return item;
            }

            throw new InvalidOperationException("Sequence contains no matching element.");
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstasync?system-linq-asyncenumerable-firstasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-system-threading-cancellationtoken)
        public async ValueTask<T> FirstAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .FirstAsync((item, _) => new ValueTask<bool>(predicate(item)), cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstasync?system-linq-asyncenumerable-firstasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-cancellationtoken)
        public async ValueTask<T> FirstAsync(CancellationToken cancellationToken = default) =>
            await source.FirstAsync(_ => true, cancellationToken).ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstordefaultasync?system-linq-asyncenumerable-firstordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-0-system-threading-cancellationtoken)
        public async ValueTask<T?> FirstOrDefaultAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            T defaultValue,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (await predicate(item, cancellationToken).ConfigureAwait(false))
                    return item;
            }

            return defaultValue;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstordefaultasync?system-linq-asyncenumerable-firstordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-0-system-threading-cancellationtoken)
        public async ValueTask<T?> FirstOrDefaultAsync(
            Func<T, bool> predicate,
            T defaultValue,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .FirstOrDefaultAsync(
                    (item, ct) => new ValueTask<bool>(predicate(item)),
                    defaultValue,
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstordefaultasync?system-linq-asyncenumerable-firstordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-0-system-threading-cancellationtoken)
        public async ValueTask<T?> FirstOrDefaultAsync(
            T defaultValue,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .FirstOrDefaultAsync(_ => true, defaultValue, cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstordefaultasync?system-linq-asyncenumerable-firstordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        public async ValueTask<T?> FirstOrDefaultAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .FirstOrDefaultAsync(predicate, default!, cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstordefaultasync?system-linq-asyncenumerable-firstordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-system-threading-cancellationtoken)
        public async ValueTask<T?> FirstOrDefaultAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .FirstOrDefaultAsync(
                    (item, _) => new ValueTask<bool>(predicate(item)),
                    default!,
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstordefaultasync?system-linq-asyncenumerable-firstordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-cancellationtoken)
        public async ValueTask<T?> FirstOrDefaultAsync(
            CancellationToken cancellationToken = default
        ) =>
            await source
                .FirstOrDefaultAsync(_ => true, default!, cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.groupby?system-linq-asyncenumerable-groupby-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-collections-generic-iequalitycomparer((-1)))
        public IAsyncEnumerable<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(
            Func<T, CancellationToken, ValueTask<TKey>> keySelector,
            Func<T, CancellationToken, ValueTask<TElement>> elementSelector,
            IEqualityComparer<TKey>? comparer = null
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);
            ArgumentNullException.ThrowIfNull(elementSelector);

            return Iterate();

            async IAsyncEnumerable<IGrouping<TKey, TElement>> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var pairs = new List<(TKey Key, TElement Element)>();
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    var key = await keySelector(item, cancellationToken).ConfigureAwait(false);
                    var element = await elementSelector(item, cancellationToken)
                        .ConfigureAwait(false);

                    pairs.Add((key, element));
                }

                foreach (var group in pairs.GroupBy(p => p.Key, p => p.Element, comparer))
                    yield return group;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.groupby?system-linq-asyncenumerable-groupby-4(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-func((-1-system-collections-generic-ienumerable((-2))-system-threading-cancellationtoken-system-threading-tasks-valuetask((-3))))-system-collections-generic-iequalitycomparer((-1)))
        public IAsyncEnumerable<TResult> GroupBy<TKey, TElement, TResult>(
            Func<T, CancellationToken, ValueTask<TKey>> keySelector,
            Func<T, CancellationToken, ValueTask<TElement>> elementSelector,
            Func<TKey, IEnumerable<TElement>, CancellationToken, ValueTask<TResult>> resultSelector,
            IEqualityComparer<TKey>? comparer = null
        ) =>
            source
                .GroupBy(keySelector, elementSelector, comparer)
                .Select((group, _, ct) => resultSelector(group.Key, group, ct));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.groupby?system-linq-asyncenumerable-groupby-4(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1))-system-func((-0-2))-system-func((-1-system-collections-generic-ienumerable((-2))-3))-system-collections-generic-iequalitycomparer((-1)))
        public IAsyncEnumerable<TResult> GroupBy<TKey, TElement, TResult>(
            Func<T, TKey> keySelector,
            Func<T, TElement> elementSelector,
            Func<TKey, IEnumerable<TElement>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer = null
        ) =>
            source.GroupBy(
                (item, _) => new ValueTask<TKey>(keySelector(item)),
                (item, _) => new ValueTask<TElement>(elementSelector(item)),
                (key, elements, _) => new ValueTask<TResult>(resultSelector(key, elements)),
                comparer
            );

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.groupby?system-linq-asyncenumerable-groupby-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1))-system-func((-0-2))-system-collections-generic-iequalitycomparer((-1)))
        public IAsyncEnumerable<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(
            Func<T, TKey> keySelector,
            Func<T, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer = null
        ) =>
            source.GroupBy(
                (item, _) => new ValueTask<TKey>(keySelector(item)),
                (item, _) => new ValueTask<TElement>(elementSelector(item)),
                comparer
            );

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.groupby?system-linq-asyncenumerable-groupby-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-func((-1-system-collections-generic-ienumerable((-0))-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-collections-generic-iequalitycomparer((-1)))
        public IAsyncEnumerable<TResult> GroupBy<TKey, TResult>(
            Func<T, CancellationToken, ValueTask<TKey>> keySelector,
            Func<TKey, IEnumerable<T>, CancellationToken, ValueTask<TResult>> resultSelector,
            IEqualityComparer<TKey>? comparer = null
        ) =>
            source.GroupBy(
                keySelector,
                (item, _) => new ValueTask<T>(item),
                resultSelector,
                comparer
            );

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.groupby?system-linq-asyncenumerable-groupby-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1))-system-func((-1-system-collections-generic-ienumerable((-0))-2))-system-collections-generic-iequalitycomparer((-1)))
        public IAsyncEnumerable<TResult> GroupBy<TKey, TResult>(
            Func<T, TKey> keySelector,
            Func<TKey, IEnumerable<T>, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer = null
        ) =>
            source.GroupBy(
                (item, _) => new ValueTask<TKey>(keySelector(item)),
                (key, items, _) => new ValueTask<TResult>(resultSelector(key, items)),
                comparer
            );

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.groupby?system-linq-asyncenumerable-groupby-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-collections-generic-iequalitycomparer((-1)))
        public IAsyncEnumerable<IGrouping<TKey, T>> GroupBy<TKey>(
            Func<T, CancellationToken, ValueTask<TKey>> keySelector,
            IEqualityComparer<TKey>? comparer = null
        ) => source.GroupBy(keySelector, (item, _) => new ValueTask<T>(item), comparer);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.groupby?system-linq-asyncenumerable-groupby-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-iequalitycomparer((-1)))
        public IAsyncEnumerable<IGrouping<TKey, T>> GroupBy<TKey>(
            Func<T, TKey> keySelector,
            IEqualityComparer<TKey>? comparer = null
        ) => source.GroupBy((item, _) => new ValueTask<TKey>(keySelector(item)), comparer);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.join?system-linq-asyncenumerable-join-4(system-collections-generic-iasyncenumerable((-0))-system-collections-generic-iasyncenumerable((-1))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-func((-1-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-func((-0-1-system-threading-cancellationtoken-system-threading-tasks-valuetask((-3))))-system-collections-generic-iequalitycomparer((-2)))
        public IAsyncEnumerable<TResult> Join<TInner, TKey, TResult>(
            IAsyncEnumerable<TInner> inner,
            Func<T, CancellationToken, ValueTask<TKey>> outerKeySelector,
            Func<TInner, CancellationToken, ValueTask<TKey>> innerKeySelector,
            Func<T, TInner, CancellationToken, ValueTask<TResult>> resultSelector,
            IEqualityComparer<TKey>? comparer = null
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(inner);
            ArgumentNullException.ThrowIfNull(outerKeySelector);
            ArgumentNullException.ThrowIfNull(innerKeySelector);
            ArgumentNullException.ThrowIfNull(resultSelector);

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var keyedInnerItems = new List<(TKey Key, TInner Item)>();
                await foreach (
                    var innerItem in inner.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    var innerKey = await innerKeySelector(innerItem, cancellationToken)
                        .ConfigureAwait(false);

                    keyedInnerItems.Add((innerKey, innerItem));
                }

                var lookup = keyedInnerItems.ToLookup(
                    item => item.Key,
                    item => item.Item,
                    comparer
                );

                await foreach (
                    var outerItem in source
                        .WithCancellation(cancellationToken)
                        .ConfigureAwait(false)
                )
                {
                    var key = await outerKeySelector(outerItem, cancellationToken)
                        .ConfigureAwait(false);

                    if (key is null)
                        continue;

                    foreach (var innerItem in lookup[key])
                    {
                        yield return await resultSelector(outerItem, innerItem, cancellationToken)
                            .ConfigureAwait(false);
                    }
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.join?system-linq-asyncenumerable-join-4(system-collections-generic-iasyncenumerable((-0))-system-collections-generic-iasyncenumerable((-1))-system-func((-0-2))-system-func((-1-2))-system-func((-0-1-3))-system-collections-generic-iequalitycomparer((-2)))
        public IAsyncEnumerable<TResult> Join<TInner, TKey, TResult>(
            IAsyncEnumerable<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer = null
        ) =>
            source.Join(
                inner,
                (outerItem, _) => new ValueTask<TKey>(outerKeySelector(outerItem)),
                (innerItem, _) => new ValueTask<TKey>(innerKeySelector(innerItem)),
                (outerItem, innerItem, _) =>
                    new ValueTask<TResult>(resultSelector(outerItem, innerItem)),
                comparer
            );

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.join?system-linq-asyncenumerable-join-3(system-collections-generic-iasyncenumerable((-0))-system-collections-generic-iasyncenumerable((-1))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-func((-1-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-collections-generic-iequalitycomparer((-2)))
        public IAsyncEnumerable<(T Outer, TInner Inner)> Join<TInner, TKey>(
            IAsyncEnumerable<TInner> inner,
            Func<T, CancellationToken, ValueTask<TKey>> outerKeySelector,
            Func<TInner, CancellationToken, ValueTask<TKey>> innerKeySelector,
            IEqualityComparer<TKey>? comparer = null
        ) =>
            source.Join(
                inner,
                outerKeySelector,
                innerKeySelector,
                (outerItem, innerItem, _) =>
                    new ValueTask<(T Outer, TInner Inner)>((outerItem, innerItem)),
                comparer
            );

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.join?system-linq-asyncenumerable-join-3(system-collections-generic-iasyncenumerable((-0))-system-collections-generic-iasyncenumerable((-1))-system-func((-0-2))-system-func((-1-2))-system-collections-generic-iequalitycomparer((-2)))
        public IAsyncEnumerable<(T Outer, TInner Inner)> Join<TInner, TKey>(
            IAsyncEnumerable<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            IEqualityComparer<TKey>? comparer = null
        ) =>
            source.Join(
                inner,
                (outerItem, _) => new ValueTask<TKey>(outerKeySelector(outerItem)),
                (innerItem, _) => new ValueTask<TKey>(innerKeySelector(innerItem)),
                comparer
            );

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.lastasync?system-linq-asyncenumerable-lastasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        public async ValueTask<T> LastAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            var hasValue = false;
            var last = default(T)!;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (await predicate(item, cancellationToken).ConfigureAwait(false))
                {
                    last = item;
                    hasValue = true;
                }
            }

            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no matching element.");

            return last;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.lastasync?system-linq-asyncenumerable-lastasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-system-threading-cancellationtoken)
        public async ValueTask<T> LastAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .LastAsync((item, _) => new ValueTask<bool>(predicate(item)), cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.lastasync?system-linq-asyncenumerable-lastasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-cancellationtoken)
        public async ValueTask<T> LastAsync(CancellationToken cancellationToken = default) =>
            await source.LastAsync(_ => true, cancellationToken).ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.lastordefaultasync?system-linq-asyncenumerable-lastordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-0-system-threading-cancellationtoken)
        public async ValueTask<T?> LastOrDefaultAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            T defaultValue,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            var hasValue = false;
            var last = default(T)!;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (await predicate(item, cancellationToken).ConfigureAwait(false))
                {
                    last = item;
                    hasValue = true;
                }
            }

            return hasValue ? last : defaultValue;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.lastordefaultasync?system-linq-asyncenumerable-lastordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-0-system-threading-cancellationtoken)
        public async ValueTask<T?> LastOrDefaultAsync(
            Func<T, bool> predicate,
            T defaultValue,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .LastOrDefaultAsync(
                    (item, _) => new ValueTask<bool>(predicate(item)),
                    defaultValue,
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.lastordefaultasync?system-linq-asyncenumerable-lastordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-0-system-threading-cancellationtoken)
        public async ValueTask<T?> LastOrDefaultAsync(
            T defaultValue,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .LastOrDefaultAsync(_ => true, defaultValue, cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.lastordefaultasync?system-linq-asyncenumerable-lastordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        public async ValueTask<T?> LastOrDefaultAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .LastOrDefaultAsync(predicate, default!, cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.lastordefaultasync?system-linq-asyncenumerable-lastordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-system-threading-cancellationtoken)
        public async ValueTask<T?> LastOrDefaultAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .LastOrDefaultAsync(
                    (item, _) => new ValueTask<bool>(predicate(item)),
                    default!,
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.lastordefaultasync?system-linq-asyncenumerable-lastordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-cancellationtoken)
        public async ValueTask<T?> LastOrDefaultAsync(
            CancellationToken cancellationToken = default
        ) =>
            await source
                .LastOrDefaultAsync(_ => true, default!, cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.longcountasync?system-linq-asyncenumerable-longcountasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        public async ValueTask<long> LongCountAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            var count = 0L;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (await predicate(item, cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.longcountasync?system-linq-asyncenumerable-longcountasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-system-threading-cancellationtoken)
        public async ValueTask<long> LongCountAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .LongCountAsync(
                    (item, _) => new ValueTask<bool>(predicate(item)),
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.longcountasync?system-linq-asyncenumerable-longcountasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-cancellationtoken)
        public async ValueTask<long> LongCountAsync(
            CancellationToken cancellationToken = default
        ) => await source.LongCountAsync(_ => true, cancellationToken).ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.maxasync
        public async ValueTask<T?> MaxAsync(
            IComparer<T>? comparer = null,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);

            var hasValue = false;
            var result = default(T);
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (item is null)
                    continue;

                if (!hasValue || (comparer ?? Comparer<T>.Default).Compare(item, result!) > 0)
                    result = item;

                hasValue = true;
            }

            if (!hasValue && default(T) is not null)
                throw new InvalidOperationException("Sequence contains no elements.");

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.minasync
        public async ValueTask<T?> MinAsync(
            IComparer<T>? comparer = null,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);

            var hasValue = false;
            var result = default(T);
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (item is null)
                    continue;

                if (!hasValue || (comparer ?? Comparer<T>.Default).Compare(item, result!) < 0)
                    result = item;

                hasValue = true;
            }

            if (!hasValue && default(T) is not null)
                throw new InvalidOperationException("Sequence contains no elements.");

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.orderby?system-linq-asyncenumerable-orderby-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-collections-generic-icomparer((-1)))
        public IOrderedAsyncEnumerable<T> OrderBy<TKey>(
            Func<T, CancellationToken, ValueTask<TKey>> keySelector,
            IComparer<TKey>? comparer = null
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);

            return new OrderedAsyncEnumerable<T>(
                source,
                async (items, cancellationToken) =>
                {
                    var keys = new List<TKey>();
                    foreach (var item in items)
                        keys.Add(await keySelector(item, cancellationToken).ConfigureAwait(false));

                    var index = 0;
                    return items.OrderBy(_ => keys[index++], comparer);
                }
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.orderby?system-linq-asyncenumerable-orderby-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))
        public IOrderedAsyncEnumerable<T> OrderBy<TKey>(
            Func<T, TKey> keySelector,
            IComparer<TKey>? comparer = null
        ) => source.OrderBy((item, _) => new ValueTask<TKey>(keySelector(item)), comparer);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.orderbydescending?system-linq-asyncenumerable-orderbydescending-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-collections-generic-icomparer((-1)))
        public IOrderedAsyncEnumerable<T> OrderByDescending<TKey>(
            Func<T, CancellationToken, ValueTask<TKey>> keySelector,
            IComparer<TKey>? comparer = null
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);

            return new OrderedAsyncEnumerable<T>(
                source,
                async (items, cancellationToken) =>
                {
                    var keys = new List<TKey>();
                    foreach (var item in items)
                        keys.Add(await keySelector(item, cancellationToken).ConfigureAwait(false));

                    var index = 0;
                    return items.OrderByDescending(_ => keys[index++], comparer);
                }
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.orderbydescending?system-linq-asyncenumerable-orderbydescending-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))
        public IOrderedAsyncEnumerable<T> OrderByDescending<TKey>(
            Func<T, TKey> keySelector,
            IComparer<TKey>? comparer = null
        ) =>
            source.OrderByDescending((item, _) => new ValueTask<TKey>(keySelector(item)), comparer);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.prepend
        public IAsyncEnumerable<T> Prepend(T element)
        {
            ArgumentNullException.ThrowIfNull(source);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                yield return element;

                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                    yield return item;
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.reverse
        public IAsyncEnumerable<T> Reverse()
        {
            ArgumentNullException.ThrowIfNull(source);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var items = await source.ToListAsync(cancellationToken).ConfigureAwait(false);
                for (var i = items.Count - 1; i >= 0; i--)
                    yield return items[i];
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.select?system-linq-asyncenumerable-select-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1)))))
        public IAsyncEnumerable<TResult> Select<TResult>(
            Func<T, int, CancellationToken, ValueTask<TResult>> selector
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(selector);

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var index = 0;
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    yield return await selector(item, checked(index++), cancellationToken)
                        .ConfigureAwait(false);
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.select?system-linq-asyncenumerable-select-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-1)))
        public IAsyncEnumerable<TResult> Select<TResult>(Func<T, int, TResult> selector) =>
            source.Select((item, index, _) => new ValueTask<TResult>(selector(item, index)));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.select?system-linq-asyncenumerable-select-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1)))))
        public IAsyncEnumerable<TResult> Select<TResult>(
            Func<T, CancellationToken, ValueTask<TResult>> selector
        ) => source.Select((item, _, ct) => selector(item, ct));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.select?system-linq-asyncenumerable-select-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1)))
        public IAsyncEnumerable<TResult> Select<TResult>(Func<T, TResult> selector) =>
            source.Select((item, _, _) => new ValueTask<TResult>(selector(item)));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-collections-generic-ienumerable((-1))))))-system-func((-0-1-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2)))))
        public IAsyncEnumerable<TResult> SelectMany<TCollection, TResult>(
            Func<T, int, CancellationToken, ValueTask<IEnumerable<TCollection>>> collectionSelector,
            Func<T, TCollection, CancellationToken, ValueTask<TResult>> resultSelector
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(collectionSelector);
            ArgumentNullException.ThrowIfNull(resultSelector);

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var index = 0;
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    var collection = await collectionSelector(
                            item,
                            checked(index++),
                            cancellationToken
                        )
                        .ConfigureAwait(false);

                    foreach (var collectionItem in collection)
                    {
                        yield return await resultSelector(item, collectionItem, cancellationToken)
                            .ConfigureAwait(false);
                    }
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-collections-generic-iasyncenumerable((-1))))-system-func((-0-1-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2)))))
        public IAsyncEnumerable<TResult> SelectMany<TCollection, TResult>(
            Func<T, int, IAsyncEnumerable<TCollection>> collectionSelector,
            Func<T, TCollection, CancellationToken, ValueTask<TResult>> resultSelector
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(collectionSelector);
            ArgumentNullException.ThrowIfNull(resultSelector);

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var index = 0;
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    await foreach (
                        var collectionItem in collectionSelector(item, checked(index++))
                            .WithCancellation(cancellationToken)
                            .ConfigureAwait(false)
                    )
                    {
                        yield return await resultSelector(item, collectionItem, cancellationToken)
                            .ConfigureAwait(false);
                    }
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-collections-generic-ienumerable((-1))))))-system-func((-0-1-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2)))))
        public IAsyncEnumerable<TResult> SelectMany<TCollection, TResult>(
            Func<T, CancellationToken, ValueTask<IEnumerable<TCollection>>> collectionSelector,
            Func<T, TCollection, CancellationToken, ValueTask<TResult>> resultSelector
        ) => source.SelectMany((item, _, ct) => collectionSelector(item, ct), resultSelector);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-collections-generic-iasyncenumerable((-1))))-system-func((-0-1-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2)))))
        public IAsyncEnumerable<TResult> SelectMany<TCollection, TResult>(
            Func<T, IAsyncEnumerable<TCollection>> collectionSelector,
            Func<T, TCollection, CancellationToken, ValueTask<TResult>> resultSelector
        ) => source.SelectMany((item, _) => collectionSelector(item), resultSelector);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-collections-generic-ienumerable((-1))))-system-func((-0-1-2)))
        public IAsyncEnumerable<TResult> SelectMany<TCollection, TResult>(
            Func<T, int, IEnumerable<TCollection>> collectionSelector,
            Func<T, TCollection, TResult> resultSelector
        ) =>
            source.SelectMany(
                (item, index, _) =>
                    new ValueTask<IEnumerable<TCollection>>(collectionSelector(item, index)),
                (item, collectionItem, _) =>
                    new ValueTask<TResult>(resultSelector(item, collectionItem))
            );

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-collections-generic-ienumerable((-1))))-system-func((-0-1-2)))
        public IAsyncEnumerable<TResult> SelectMany<TCollection, TResult>(
            Func<T, IEnumerable<TCollection>> collectionSelector,
            Func<T, TCollection, TResult> resultSelector
        ) => source.SelectMany((item, _) => collectionSelector(item), resultSelector);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-collections-generic-iasyncenumerable((-1))))-system-func((-0-1-2)))
        public IAsyncEnumerable<TResult> SelectMany<TCollection, TResult>(
            Func<T, IAsyncEnumerable<TCollection>> collectionSelector,
            Func<T, TCollection, TResult> resultSelector
        ) =>
            source.SelectMany(
                collectionSelector,
                (item, collectionItem, _) =>
                    new ValueTask<TResult>(resultSelector(item, collectionItem))
            );

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-collections-generic-ienumerable((-1)))))))
        public IAsyncEnumerable<TResult> SelectMany<TResult>(
            Func<T, int, CancellationToken, ValueTask<IEnumerable<TResult>>> selector
        ) => source.SelectMany(selector, (_, item, _) => new ValueTask<TResult>(item));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-collections-generic-iasyncenumerable((-1)))))
        public IAsyncEnumerable<TResult> SelectMany<TResult>(
            Func<T, int, IAsyncEnumerable<TResult>> selector
        ) => source.SelectMany(selector, (_, item, _) => new ValueTask<TResult>(item));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-collections-generic-ienumerable((-1)))))))
        public IAsyncEnumerable<TResult> SelectMany<TResult>(
            Func<T, CancellationToken, ValueTask<IEnumerable<TResult>>> selector
        ) => source.SelectMany(selector, (_, item, _) => new ValueTask<TResult>(item));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-collections-generic-ienumerable((-1)))))
        public IAsyncEnumerable<TResult> SelectMany<TResult>(
            Func<T, int, IEnumerable<TResult>> selector
        ) => source.SelectMany(selector, (_, item) => item);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-collections-generic-iasyncenumerable((-1)))))
        public IAsyncEnumerable<TResult> SelectMany<TResult>(
            Func<T, IAsyncEnumerable<TResult>> selector
        ) => source.SelectMany(selector, (_, item, _) => new ValueTask<TResult>(item));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany?system-linq-asyncenumerable-selectmany-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-collections-generic-ienumerable((-1)))))
        public IAsyncEnumerable<TResult> SelectMany<TResult>(
            Func<T, IEnumerable<TResult>> selector
        ) => source.SelectMany(selector, (_, item) => item);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sequenceequalasync
        public async ValueTask<bool> SequenceEqualAsync(
            IAsyncEnumerable<T> second,
            IEqualityComparer<T>? comparer = null,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(second);

            await using var firstEnumerator = source.GetAsyncEnumerator(cancellationToken);
            await using var secondEnumerator = second.GetAsyncEnumerator(cancellationToken);

            while (true)
            {
                var hasFirst = await firstEnumerator.MoveNextAsync().ConfigureAwait(false);
                var hasSecond = await secondEnumerator.MoveNextAsync().ConfigureAwait(false);

                if (hasFirst != hasSecond)
                    return false;
                if (!hasFirst)
                    return true;

                if (
                    !(comparer ?? EqualityComparer<T>.Default).Equals(
                        firstEnumerator.Current,
                        secondEnumerator.Current
                    )
                )
                {
                    return false;
                }
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.singleasync?system-linq-asyncenumerable-singleasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        public async ValueTask<T> SingleAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            var hasValue = false;
            var result = default(T)!;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (!await predicate(item, cancellationToken).ConfigureAwait(false))
                    continue;

                if (hasValue)
                {
                    throw new InvalidOperationException(
                        "Sequence contains more than one matching element."
                    );
                }

                result = item;
                hasValue = true;
            }

            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no matching element.");

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.singleasync?system-linq-asyncenumerable-singleasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-system-threading-cancellationtoken)
        public async ValueTask<T> SingleAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .SingleAsync((item, _) => new ValueTask<bool>(predicate(item)), cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.singleasync?system-linq-asyncenumerable-singleasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-cancellationtoken)
        public async ValueTask<T> SingleAsync(CancellationToken cancellationToken = default) =>
            await source.SingleAsync(_ => true, cancellationToken).ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.singleordefaultasync?system-linq-asyncenumerable-singleordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-0-system-threading-cancellationtoken)
        public async ValueTask<T?> SingleOrDefaultAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            T defaultValue,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            var hasValue = false;
            var result = default(T)!;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (!await predicate(item, cancellationToken).ConfigureAwait(false))
                    continue;

                if (hasValue)
                {
                    throw new InvalidOperationException(
                        "Sequence contains more than one matching element."
                    );
                }

                result = item;
                hasValue = true;
            }

            return hasValue ? result : defaultValue;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.singleordefaultasync?system-linq-asyncenumerable-singleordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-0-system-threading-cancellationtoken)
        public async ValueTask<T?> SingleOrDefaultAsync(
            Func<T, bool> predicate,
            T defaultValue,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .SingleOrDefaultAsync(
                    (item, _) => new ValueTask<bool>(predicate(item)),
                    defaultValue,
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.singleordefaultasync?system-linq-asyncenumerable-singleordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-0-system-threading-cancellationtoken)
        public async ValueTask<T?> SingleOrDefaultAsync(
            T defaultValue,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .SingleOrDefaultAsync(_ => true, defaultValue, cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.singleordefaultasync?system-linq-asyncenumerable-singleordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean))))-system-threading-cancellationtoken)
        public async ValueTask<T?> SingleOrDefaultAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .SingleOrDefaultAsync(predicate, default!, cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.singleordefaultasync?system-linq-asyncenumerable-singleordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean))-system-threading-cancellationtoken)
        public async ValueTask<T?> SingleOrDefaultAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .SingleOrDefaultAsync(
                    (item, _) => new ValueTask<bool>(predicate(item)),
                    default!,
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.singleordefaultasync?system-linq-asyncenumerable-singleordefaultasync-1(system-collections-generic-iasyncenumerable((-0))-system-threading-cancellationtoken)
        public async ValueTask<T?> SingleOrDefaultAsync(
            CancellationToken cancellationToken = default
        ) =>
            await source
                .SingleOrDefaultAsync(_ => true, default!, cancellationToken)
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.skip
        public IAsyncEnumerable<T> Skip(int count)
        {
            ArgumentNullException.ThrowIfNull(source);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var skipped = 0;
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (skipped < count)
                        skipped++;
                    else
                        yield return item;
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.skipwhile?system-linq-asyncenumerable-skipwhile-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean)))))
        public IAsyncEnumerable<T> SkipWhile(
            Func<T, int, CancellationToken, ValueTask<bool>> predicate
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var index = 0;
                var skipping = true;
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (
                        skipping
                        && await predicate(item, checked(index++), cancellationToken)
                            .ConfigureAwait(false)
                    )
                    {
                        continue;
                    }

                    skipping = false;
                    yield return item;
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.skipwhile?system-linq-asyncenumerable-skipwhile-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-boolean)))
        public IAsyncEnumerable<T> SkipWhile(Func<T, int, bool> predicate) =>
            source.SkipWhile((item, index, _) => new ValueTask<bool>(predicate(item, index)));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.skipwhile?system-linq-asyncenumerable-skipwhile-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean)))))
        public IAsyncEnumerable<T> SkipWhile(
            Func<T, CancellationToken, ValueTask<bool>> predicate
        ) => source.SkipWhile((item, _, ct) => predicate(item, ct));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.skipwhile?system-linq-asyncenumerable-skipwhile-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean)))
        public IAsyncEnumerable<T> SkipWhile(Func<T, bool> predicate) =>
            source.SkipWhile((item, _, _) => new ValueTask<bool>(predicate(item)));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.take
        public IAsyncEnumerable<T> Take(int count)
        {
            ArgumentNullException.ThrowIfNull(source);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                if (count <= 0)
                    yield break;

                var taken = 0;
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    yield return item;

                    if (++taken >= count)
                        yield break;
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.takewhile?system-linq-asyncenumerable-takewhile-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean)))))
        public IAsyncEnumerable<T> TakeWhile(
            Func<T, int, CancellationToken, ValueTask<bool>> predicate
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var index = 0;
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (
                        !await predicate(item, checked(index++), cancellationToken)
                            .ConfigureAwait(false)
                    )
                    {
                        yield break;
                    }

                    yield return item;
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.takewhile?system-linq-asyncenumerable-takewhile-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-boolean)))
        public IAsyncEnumerable<T> TakeWhile(Func<T, int, bool> predicate) =>
            source.TakeWhile((item, index, _) => new ValueTask<bool>(predicate(item, index)));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.takewhile?system-linq-asyncenumerable-takewhile-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean)))))
        public IAsyncEnumerable<T> TakeWhile(
            Func<T, CancellationToken, ValueTask<bool>> predicate
        ) => source.TakeWhile((item, _, ct) => predicate(item, ct));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.takewhile?system-linq-asyncenumerable-takewhile-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean)))
        public IAsyncEnumerable<T> TakeWhile(Func<T, bool> predicate) =>
            source.TakeWhile((item, _, _) => new ValueTask<bool>(predicate(item)));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.toarrayasync
        public async ValueTask<T[]> ToArrayAsync(CancellationToken cancellationToken = default) =>
            (await source.ToListAsync(cancellationToken).ConfigureAwait(false)).ToArray();

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.todictionaryasync?system-linq-asyncenumerable-todictionaryasync-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-collections-generic-iequalitycomparer((-1))-system-threading-cancellationtoken)
        public async ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(
            Func<T, CancellationToken, ValueTask<TKey>> keySelector,
            Func<T, CancellationToken, ValueTask<TElement>> elementSelector,
            IEqualityComparer<TKey>? comparer = null,
            CancellationToken cancellationToken = default
        )
            where TKey : notnull
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);
            ArgumentNullException.ThrowIfNull(elementSelector);

            var result = new Dictionary<TKey, TElement>(comparer);
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                var key = await keySelector(item, cancellationToken).ConfigureAwait(false);
                var element = await elementSelector(item, cancellationToken).ConfigureAwait(false);

                result.Add(key, element);
            }

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.todictionaryasync?system-linq-asyncenumerable-todictionaryasync-3(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1))-system-func((-0-2))-system-collections-generic-iequalitycomparer((-1))-system-threading-cancellationtoken)
        public async ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(
            Func<T, TKey> keySelector,
            Func<T, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer = null,
            CancellationToken cancellationToken = default
        )
            where TKey : notnull =>
            await source
                .ToDictionaryAsync(
                    (item, _) => new ValueTask<TKey>(keySelector(item)),
                    (item, _) => new ValueTask<TElement>(elementSelector(item)),
                    comparer,
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.todictionaryasync?system-linq-asyncenumerable-todictionaryasync-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-collections-generic-iequalitycomparer((-1))-system-threading-cancellationtoken)
        public async ValueTask<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(
            Func<T, CancellationToken, ValueTask<TKey>> keySelector,
            IEqualityComparer<TKey>? comparer = null,
            CancellationToken cancellationToken = default
        )
            where TKey : notnull =>
            await source
                .ToDictionaryAsync(
                    keySelector,
                    (item, _) => new ValueTask<T>(item),
                    comparer,
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.todictionaryasync?system-linq-asyncenumerable-todictionaryasync-2(system-collections-generic-iasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-iequalitycomparer((-1))-system-threading-cancellationtoken)
        public async ValueTask<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(
            Func<T, TKey> keySelector,
            IEqualityComparer<TKey>? comparer = null,
            CancellationToken cancellationToken = default
        )
            where TKey : notnull =>
            await source
                .ToDictionaryAsync(
                    (item, _) => new ValueTask<TKey>(keySelector(item)),
                    comparer,
                    cancellationToken
                )
                .ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.tohashsetasync
        public async ValueTask<HashSet<T>> ToHashSetAsync(
            IEqualityComparer<T>? comparer = null,
            CancellationToken cancellationToken = default
        )
        {
            ArgumentNullException.ThrowIfNull(source);

            var result = new HashSet<T>(comparer);
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                result.Add(item);
            }

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.tolistasync
        public async ValueTask<List<T>> ToListAsync(CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(source);

            var result = new List<T>();
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                result.Add(item);
            }

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.union
        public IAsyncEnumerable<T> Union(
            IAsyncEnumerable<T> second,
            IEqualityComparer<T>? comparer = null
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(second);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var seen = new HashSet<T>(comparer);
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (seen.Add(item))
                        yield return item;
                }

                await foreach (
                    var item in second.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (seen.Add(item))
                        yield return item;
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.where?system-linq-asyncenumerable-where-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean)))))
        public IAsyncEnumerable<T> Where(Func<T, int, CancellationToken, ValueTask<bool>> predicate)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var index = 0;
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (
                        await predicate(item, checked(index++), cancellationToken)
                            .ConfigureAwait(false)
                    )
                    {
                        yield return item;
                    }
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.where?system-linq-asyncenumerable-where-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-int32-system-boolean)))
        public IAsyncEnumerable<T> Where(Func<T, int, bool> predicate) =>
            source.Where((item, index, _) => new ValueTask<bool>(predicate(item, index)));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.where?system-linq-asyncenumerable-where-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((system-boolean)))))
        public IAsyncEnumerable<T> Where(Func<T, CancellationToken, ValueTask<bool>> predicate) =>
            source.Where((item, _, ct) => predicate(item, ct));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.where?system-linq-asyncenumerable-where-1(system-collections-generic-iasyncenumerable((-0))-system-func((-0-system-boolean)))
        public IAsyncEnumerable<T> Where(Func<T, bool> predicate) =>
            source.Where((item, _, _) => new ValueTask<bool>(predicate(item)));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.zip?system-linq-asyncenumerable-zip-3(system-collections-generic-iasyncenumerable((-0))-system-collections-generic-iasyncenumerable((-1))-system-func((-0-1-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2)))))
        public IAsyncEnumerable<TResult> Zip<TSecond, TResult>(
            IAsyncEnumerable<TSecond> second,
            Func<T, TSecond, CancellationToken, ValueTask<TResult>> resultSelector
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(second);
            ArgumentNullException.ThrowIfNull(resultSelector);

            return Iterate();

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await using var firstEnumerator = source.GetAsyncEnumerator(cancellationToken);
                await using var secondEnumerator = second.GetAsyncEnumerator(cancellationToken);

                while (
                    await firstEnumerator.MoveNextAsync().ConfigureAwait(false)
                    && await secondEnumerator.MoveNextAsync().ConfigureAwait(false)
                )
                {
                    yield return await resultSelector(
                            firstEnumerator.Current,
                            secondEnumerator.Current,
                            cancellationToken
                        )
                        .ConfigureAwait(false);
                }
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.zip?system-linq-asyncenumerable-zip-3(system-collections-generic-iasyncenumerable((-0))-system-collections-generic-iasyncenumerable((-1))-system-collections-generic-iasyncenumerable((-2)))
        public IAsyncEnumerable<(T First, TSecond Second, TThird Third)> Zip<TSecond, TThird>(
            IAsyncEnumerable<TSecond> second,
            IAsyncEnumerable<TThird> third
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(second);
            ArgumentNullException.ThrowIfNull(third);

            async IAsyncEnumerable<(T First, TSecond Second, TThird Third)> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await using var firstEnumerator = source.GetAsyncEnumerator(cancellationToken);
                await using var secondEnumerator = second.GetAsyncEnumerator(cancellationToken);
                await using var thirdEnumerator = third.GetAsyncEnumerator(cancellationToken);

                while (
                    await firstEnumerator.MoveNextAsync().ConfigureAwait(false)
                    && await secondEnumerator.MoveNextAsync().ConfigureAwait(false)
                    && await thirdEnumerator.MoveNextAsync().ConfigureAwait(false)
                )
                {
                    yield return (
                        firstEnumerator.Current,
                        secondEnumerator.Current,
                        thirdEnumerator.Current
                    );
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.zip?system-linq-asyncenumerable-zip-3(system-collections-generic-iasyncenumerable((-0))-system-collections-generic-iasyncenumerable((-1))-system-func((-0-1-2)))
        public IAsyncEnumerable<TResult> Zip<TSecond, TResult>(
            IAsyncEnumerable<TSecond> second,
            Func<T, TSecond, TResult> resultSelector
        ) => source.Zip(second, (a, b, _) => new ValueTask<TResult>(resultSelector(a, b)));

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.zip?system-linq-asyncenumerable-zip-2(system-collections-generic-iasyncenumerable((-0))-system-collections-generic-iasyncenumerable((-1)))
        public IAsyncEnumerable<(T First, TSecond Second)> Zip<TSecond>(
            IAsyncEnumerable<TSecond> second
        ) => source.Zip(second, (a, b) => (a, b));
    }

    extension(IAsyncEnumerable<object?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.cast
        public IAsyncEnumerable<TResult> Cast<TResult>()
        {
            ArgumentNullException.ThrowIfNull(source);

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    yield return (TResult)item!;
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.oftype
        public IAsyncEnumerable<TResult> OfType<TResult>()
        {
            ArgumentNullException.ThrowIfNull(source);

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (item is TResult result)
                        yield return result;
                }
            }

            return Iterate();
        }
    }

    extension<TKey, TValue>(IAsyncEnumerable<KeyValuePair<TKey, TValue>> source)
        where TKey : notnull
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.todictionaryasync?system-linq-asyncenumerable-todictionaryasync-2(system-collections-generic-iasyncenumerable((system-collections-generic-keyvaluepair((-0-1))))-system-collections-generic-iequalitycomparer((-0))-system-threading-cancellationtoken)
        public async ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync(
            IEqualityComparer<TKey>? comparer = null,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .ToDictionaryAsync(
                    pair => pair.Key,
                    pair => pair.Value,
                    comparer,
                    cancellationToken
                )
                .ConfigureAwait(false);
    }

    extension<TKey, TValue>(IAsyncEnumerable<(TKey Key, TValue Value)> source)
        where TKey : notnull
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.todictionaryasync?system-linq-asyncenumerable-todictionaryasync-2(system-collections-generic-iasyncenumerable((system-valuetuple((-0-1))))-system-collections-generic-iequalitycomparer((-0))-system-threading-cancellationtoken)
        public async ValueTask<Dictionary<TKey, TValue>> ToDictionaryAsync(
            IEqualityComparer<TKey>? comparer = null,
            CancellationToken cancellationToken = default
        ) =>
            await source
                .ToDictionaryAsync(
                    pair => pair.Key,
                    pair => pair.Value,
                    comparer,
                    cancellationToken
                )
                .ConfigureAwait(false);
    }

    extension(IAsyncEnumerable<int> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync?system-linq-asyncenumerable-sumasync(system-collections-generic-iasyncenumerable((system-int32))-system-threading-cancellationtoken)
        public async ValueTask<int> SumAsync(CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(source);

            var sum = 0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                sum = checked(sum + item);
            }

            return sum;
        }
    }

    extension(IAsyncEnumerable<int?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync?system-linq-asyncenumerable-sumasync(system-collections-generic-iasyncenumerable((system-nullable((system-int32))))-system-threading-cancellationtoken)
        public async ValueTask<int?> SumAsync(CancellationToken cancellationToken = default) =>
            await source
                .Select(item => item.GetValueOrDefault())
                .SumAsync(cancellationToken)
                .ConfigureAwait(false);
    }

    extension(IAsyncEnumerable<long> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync?system-linq-asyncenumerable-sumasync(system-collections-generic-iasyncenumerable((system-int64))-system-threading-cancellationtoken)
        public async ValueTask<long> SumAsync(CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(source);

            var sum = 0L;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                sum = checked(sum + item);
            }

            return sum;
        }
    }

    extension(IAsyncEnumerable<long?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync?system-linq-asyncenumerable-sumasync(system-collections-generic-iasyncenumerable((system-nullable((system-int64))))-system-threading-cancellationtoken)
        public async ValueTask<long?> SumAsync(CancellationToken cancellationToken = default) =>
            await source
                .Select(item => item.GetValueOrDefault())
                .SumAsync(cancellationToken)
                .ConfigureAwait(false);
    }

    extension(IAsyncEnumerable<double> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync?system-linq-asyncenumerable-sumasync(system-collections-generic-iasyncenumerable((system-double))-system-threading-cancellationtoken)
        public async ValueTask<double> SumAsync(CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(source);

            var sum = 0.0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                sum += item;
            }

            return sum;
        }
    }

    extension(IAsyncEnumerable<double?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync?system-linq-asyncenumerable-sumasync(system-collections-generic-iasyncenumerable((system-nullable((system-double))))-system-threading-cancellationtoken)
        public async ValueTask<double?> SumAsync(CancellationToken cancellationToken = default) =>
            await source
                .Select(item => item.GetValueOrDefault())
                .SumAsync(cancellationToken)
                .ConfigureAwait(false);
    }

    extension(IAsyncEnumerable<float> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync?system-linq-asyncenumerable-sumasync(system-collections-generic-iasyncenumerable((system-single))-system-threading-cancellationtoken)
        public async ValueTask<float> SumAsync(CancellationToken cancellationToken = default) =>
            (float)
                await source
                    .Select(item => (double)item)
                    .SumAsync(cancellationToken)
                    .ConfigureAwait(false);
    }

    extension(IAsyncEnumerable<float?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync?system-linq-asyncenumerable-sumasync(system-collections-generic-iasyncenumerable((system-nullable((system-single))))-system-threading-cancellationtoken)
        public async ValueTask<float?> SumAsync(CancellationToken cancellationToken = default) =>
            await source
                .Select(item => item.GetValueOrDefault())
                .SumAsync(cancellationToken)
                .ConfigureAwait(false);
    }

    extension(IAsyncEnumerable<decimal> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync?system-linq-asyncenumerable-sumasync(system-collections-generic-iasyncenumerable((system-decimal))-system-threading-cancellationtoken)
        public async ValueTask<decimal> SumAsync(CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(source);

            var sum = 0m;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                sum += item;
            }

            return sum;
        }
    }

    extension(IAsyncEnumerable<decimal?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync?system-linq-asyncenumerable-sumasync(system-collections-generic-iasyncenumerable((system-nullable((system-decimal))))-system-threading-cancellationtoken)
        public async ValueTask<decimal?> SumAsync(CancellationToken cancellationToken = default) =>
            await source
                .Select(item => item.GetValueOrDefault())
                .SumAsync(cancellationToken)
                .ConfigureAwait(false);
    }

    extension<T>(IOrderedAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.thenby?system-linq-asyncenumerable-thenby-2(system-linq-iorderedasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-collections-generic-icomparer((-1)))
        public IOrderedAsyncEnumerable<T> ThenBy<TKey>(
            Func<T, CancellationToken, ValueTask<TKey>> keySelector,
            IComparer<TKey>? comparer = null
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);

            return source.CreateOrderedAsyncEnumerable(keySelector, comparer, false);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.thenby?system-linq-asyncenumerable-thenby-2(system-linq-iorderedasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))
        public IOrderedAsyncEnumerable<T> ThenBy<TKey>(
            Func<T, TKey> keySelector,
            IComparer<TKey>? comparer = null
        ) => source.ThenBy((item, _) => new ValueTask<TKey>(keySelector(item)), comparer);

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.thenbydescending?system-linq-asyncenumerable-thenbydescending-2(system-linq-iorderedasyncenumerable((-0))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-collections-generic-icomparer((-1)))
        public IOrderedAsyncEnumerable<T> ThenByDescending<TKey>(
            Func<T, CancellationToken, ValueTask<TKey>> keySelector,
            IComparer<TKey>? comparer = null
        )
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);

            return source.CreateOrderedAsyncEnumerable(keySelector, comparer, true);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.thenbydescending?system-linq-asyncenumerable-thenbydescending-2(system-linq-iorderedasyncenumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))
        public IOrderedAsyncEnumerable<T> ThenByDescending<TKey>(
            Func<T, TKey> keySelector,
            IComparer<TKey>? comparer = null
        ) => source.ThenByDescending((item, _) => new ValueTask<TKey>(keySelector(item)), comparer);
    }

    private sealed class OrderedAsyncEnumerable<T>(
        IAsyncEnumerable<T> source,
        Func<IEnumerable<T>, CancellationToken, ValueTask<IOrderedEnumerable<T>>> sorter
    ) : IOrderedAsyncEnumerable<T>
    {
        public IOrderedAsyncEnumerable<T> CreateOrderedAsyncEnumerable<TKey>(
            Func<T, TKey> keySelector,
            IComparer<TKey>? comparer,
            bool descending
        ) =>
            new OrderedAsyncEnumerable<T>(
                source,
                async (items, cancellationToken) =>
                {
                    var ordered = await sorter(items, cancellationToken).ConfigureAwait(false);
                    return ordered.CreateOrderedEnumerable(keySelector, comparer, descending);
                }
            );

        public IOrderedAsyncEnumerable<T> CreateOrderedAsyncEnumerable<TKey>(
            Func<T, CancellationToken, ValueTask<TKey>> keySelector,
            IComparer<TKey>? comparer,
            bool descending
        ) =>
            new OrderedAsyncEnumerable<T>(
                source,
                async (items, cancellationToken) =>
                {
                    var ordered = await sorter(items, cancellationToken).ConfigureAwait(false);
                    var keys = new List<TKey>();
                    foreach (var item in items)
                        keys.Add(await keySelector(item, cancellationToken).ConfigureAwait(false));

                    var index = 0;
                    return ordered.CreateOrderedEnumerable(
                        _ => keys[index++],
                        comparer,
                        descending
                    );
                }
            );

        public async IAsyncEnumerator<T> GetAsyncEnumerator(
            CancellationToken cancellationToken = default
        )
        {
            var items = await source.ToListAsync(cancellationToken).ConfigureAwait(false);
            var sortedItems = await sorter(items, cancellationToken).ConfigureAwait(false);
            foreach (var item in sortedItems)
                yield return item;
        }
    }
}
#endif
#endif
