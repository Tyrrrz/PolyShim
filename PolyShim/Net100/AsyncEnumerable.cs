#if (NETCOREAPP && !NET10_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// Task and ValueTask are not available on all target frameworks within this TFM range without a NuGet package reference
#if FEATURE_TASK && FEATURE_VALUETASK
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
    public static IAsyncEnumerable<T> Empty<T>() => EmptyIterator<T>();

    // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.range
    public static IAsyncEnumerable<int> Range(int start, int count)
    {
        if (count < 0 || (long)start + count - 1 > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(count));

        return RangeIterator(start, count);
    }

    // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.repeat
    public static IAsyncEnumerable<T> Repeat<T>(T element, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        return RepeatIterator(element, count);
    }

    extension<T>(IEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.toasyncenumerable
        public IAsyncEnumerable<T> ToAsyncEnumerable()
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return ToAsyncEnumerableIterator(source);
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.aggregate
        public async ValueTask<T> AggregateAsync(
            Func<T, T, T> accumulator,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator is null)
                throw new ArgumentNullException(nameof(accumulator));

            var hasValue = false;
            var result = default(T)!;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                result = hasValue ? accumulator(result, item) : item;
                hasValue = true;
            }

            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no elements.");

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.aggregate
        public async ValueTask<TAccumulate> AggregateAsync<TAccumulate>(
            TAccumulate seed,
            Func<TAccumulate, T, TAccumulate> accumulator,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator is null)
                throw new ArgumentNullException(nameof(accumulator));

            var result = seed;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                result = accumulator(result, item);

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.all
        public async ValueTask<bool> AllAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (!predicate(item))
                    return false;
            }

            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.allawaitasync
        public async ValueTask<bool> AllAwaitAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (!await predicate(item, cancellationToken).ConfigureAwait(false))
                    return false;
            }

            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.any
        public async ValueTask<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            await foreach (
                var _ in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                return true;

            return false;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.any
        public async ValueTask<bool> AnyAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (predicate(item))
                    return true;
            }

            return false;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.anyawaitasync
        public async ValueTask<bool> AnyAwaitAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (await predicate(item, cancellationToken).ConfigureAwait(false))
                    return true;
            }

            return false;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.append
        public IAsyncEnumerable<T> Append(T element)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return AppendIterator(source, element);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.prepend
        public IAsyncEnumerable<T> Prepend(T element)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return PrependIterator(source, element);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.concat
        public IAsyncEnumerable<T> Concat(IAsyncEnumerable<T> second)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (second is null)
                throw new ArgumentNullException(nameof(second));

            return ConcatIterator(source, second);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.contains
        public async ValueTask<bool> ContainsAsync(
            T value,
            IEqualityComparer<T>? comparer = null,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            comparer ??= EqualityComparer<T>.Default;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (comparer.Equals(item, value))
                    return true;
            }

            return false;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.count
        public async ValueTask<int> CountAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var count = 0;
            await foreach (
                var _ in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                checked
                {
                    count++;
                }

            return count;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.count
        public async ValueTask<int> CountAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            var count = 0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (predicate(item))
                    checked
                    {
                        count++;
                    }
            }

            return count;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.countawaitasync
        public async ValueTask<int> CountAwaitAsync(
            Func<T, CancellationToken, ValueTask<bool>> predicate,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            var count = 0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (await predicate(item, cancellationToken).ConfigureAwait(false))
                    checked
                    {
                        count++;
                    }
            }

            return count;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.longcount
        public async ValueTask<long> LongCountAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var count = 0L;
            await foreach (
                var _ in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                checked
                {
                    count++;
                }

            return count;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.defaultifempty
        public IAsyncEnumerable<T?> DefaultIfEmpty(T defaultValue)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return DefaultIfEmptyIterator(source, defaultValue);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.distinct
        public IAsyncEnumerable<T> Distinct(IEqualityComparer<T>? comparer = null)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return DistinctIterator(source, comparer);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.elementat
        public async ValueTask<T> ElementAtAsync(
            int index,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.elementatordefault
        public async ValueTask<T?> ElementAtOrDefaultAsync(
            int index,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.first
        public async ValueTask<T> FirstAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                return item;

            throw new InvalidOperationException("Sequence contains no elements.");
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.first
        public async ValueTask<T> FirstAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (predicate(item))
                    return item;
            }

            throw new InvalidOperationException("Sequence contains no matching element.");
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstordefault
        public async ValueTask<T?> FirstOrDefaultAsync(
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                return item;

            return default;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstordefault
        public async ValueTask<T?> FirstOrDefaultAsync(
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (predicate(item))
                    return item;
            }

            return default;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.last
        public async ValueTask<T> LastAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var hasValue = false;
            var last = default(T)!;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                last = item;
                hasValue = true;
            }

            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no elements.");

            return last;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.lastordefault
        public async ValueTask<T?> LastOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var last = default(T);
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                last = item;

            return last;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.single
        public async ValueTask<T> SingleAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var hasValue = false;
            var result = default(T)!;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (hasValue)
                    throw new InvalidOperationException("Sequence contains more than one element.");

                result = item;
                hasValue = true;
            }

            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no elements.");

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.singleordefault
        public async ValueTask<T?> SingleOrDefaultAsync(
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var hasValue = false;
            var result = default(T);
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (hasValue)
                    throw new InvalidOperationException("Sequence contains more than one element.");

                result = item;
                hasValue = true;
            }

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.max
        public async ValueTask<T?> MaxAsync(
            IComparer<T>? comparer = null,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            comparer ??= Comparer<T>.Default;
            var hasValue = false;
            var result = default(T);
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (!hasValue || comparer.Compare(item, result!) > 0)
                    result = item;

                hasValue = true;
            }

            if (!hasValue && default(T) is not null)
                throw new InvalidOperationException("Sequence contains no elements.");

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.min
        public async ValueTask<T?> MinAsync(
            IComparer<T>? comparer = null,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            comparer ??= Comparer<T>.Default;
            var hasValue = false;
            var result = default(T);
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (!hasValue || comparer.Compare(item, result!) < 0)
                    result = item;

                hasValue = true;
            }

            if (!hasValue && default(T) is not null)
                throw new InvalidOperationException("Sequence contains no elements.");

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.reverse
        public IAsyncEnumerable<T> Reverse()
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return ReverseIterator(source);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.select
        public IAsyncEnumerable<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return SelectIterator(source, selector);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.select
        public IAsyncEnumerable<TResult> Select<TResult>(Func<T, int, TResult> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return SelectIndexedIterator(source, selector);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectawait
        public IAsyncEnumerable<TResult> SelectAwait<TResult>(
            Func<T, CancellationToken, ValueTask<TResult>> selector
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return SelectAsyncIterator(source, selector);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectmany
        public IAsyncEnumerable<TResult> SelectMany<TResult>(
            Func<T, IAsyncEnumerable<TResult>> selector
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return SelectManyIterator(source, selector);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sequenceequal
        public async ValueTask<bool> SequenceEqualAsync(
            IAsyncEnumerable<T> second,
            IEqualityComparer<T>? comparer = null,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (second is null)
                throw new ArgumentNullException(nameof(second));

            comparer ??= EqualityComparer<T>.Default;

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
                if (!comparer.Equals(firstEnumerator.Current, secondEnumerator.Current))
                    return false;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.skip
        public IAsyncEnumerable<T> Skip(int count)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return SkipIterator(source, count);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.skipwhile
        public IAsyncEnumerable<T> SkipWhile(Func<T, bool> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return SkipWhileIterator(source, predicate);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.take
        public IAsyncEnumerable<T> Take(int count)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return TakeIterator(source, count);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.takewhile
        public IAsyncEnumerable<T> TakeWhile(Func<T, bool> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return TakeWhileIterator(source, predicate);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.toarray
        public async ValueTask<T[]> ToArrayAsync(CancellationToken cancellationToken = default) =>
            (await source.ToListAsync(cancellationToken).ConfigureAwait(false)).ToArray();

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.todictionary
        public async ValueTask<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(
            Func<T, TKey> keySelector,
            IEqualityComparer<TKey>? comparer = null,
            CancellationToken cancellationToken = default
        )
            where TKey : notnull
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            var result = new Dictionary<TKey, T>(comparer);
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                result.Add(keySelector(item), item);

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.tohashset
        public async ValueTask<HashSet<T>> ToHashSetAsync(
            IEqualityComparer<T>? comparer = null,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var result = new HashSet<T>(comparer);
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                result.Add(item);

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.tolist
        public async ValueTask<List<T>> ToListAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var result = new List<T>();
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                result.Add(item);

            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.where
        public IAsyncEnumerable<T> Where(Func<T, bool> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return WhereIterator(source, predicate);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.where
        public IAsyncEnumerable<T> Where(Func<T, int, bool> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return WhereIndexedIterator(source, predicate);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.whereawait
        public IAsyncEnumerable<T> WhereAwait(Func<T, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return WhereAsyncIterator(source, predicate);
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.orderby
        public IOrderedAsyncEnumerable<T> OrderBy<TKey>(
            Func<T, TKey> keySelector,
            IComparer<TKey>? comparer = null
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new OrderedAsyncEnumerable<T>(
                source,
                items => items.OrderBy(keySelector, comparer)
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.orderbydescending
        public IOrderedAsyncEnumerable<T> OrderByDescending<TKey>(
            Func<T, TKey> keySelector,
            IComparer<TKey>? comparer = null
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return new OrderedAsyncEnumerable<T>(
                source,
                items => items.OrderByDescending(keySelector, comparer)
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.groupby
        public IAsyncEnumerable<IGrouping<TKey, T>> GroupBy<TKey>(
            Func<T, TKey> keySelector,
            IEqualityComparer<TKey>? comparer = null
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return GroupByIterator(source, keySelector, comparer);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.except
        public IAsyncEnumerable<T> Except(
            IAsyncEnumerable<T> second,
            IEqualityComparer<T>? comparer = null
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (second is null)
                throw new ArgumentNullException(nameof(second));

            return ExceptIterator(source, second, comparer);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.union
        public IAsyncEnumerable<T> Union(
            IAsyncEnumerable<T> second,
            IEqualityComparer<T>? comparer = null
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (second is null)
                throw new ArgumentNullException(nameof(second));

            return UnionIterator(source, second, comparer);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.zip
        public IAsyncEnumerable<(T First, TSecond Second)> Zip<TSecond>(
            IAsyncEnumerable<TSecond> second
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (second is null)
                throw new ArgumentNullException(nameof(second));

            return ZipIterator(source, second, (a, b) => (a, b));
        }
    }

    extension<TOuter>(IAsyncEnumerable<TOuter> outer)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.join
        public IAsyncEnumerable<TResult> Join<TInner, TKey, TResult>(
            IAsyncEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer = null
        )
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));
            if (inner is null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector is null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector is null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector is null)
                throw new ArgumentNullException(nameof(resultSelector));

            return JoinIterator(
                outer,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                comparer
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.zip
        public IAsyncEnumerable<TResult> Zip<TSecond, TResult>(
            IAsyncEnumerable<TSecond> second,
            Func<TOuter, TSecond, TResult> resultSelector
        )
        {
            if (outer is null)
                throw new ArgumentNullException(nameof(outer));
            if (second is null)
                throw new ArgumentNullException(nameof(second));
            if (resultSelector is null)
                throw new ArgumentNullException(nameof(resultSelector));

            return ZipIterator(outer, second, resultSelector);
        }
    }

    extension<T>(IOrderedAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.thenby
        public IOrderedAsyncEnumerable<T> ThenBy<TKey>(
            Func<T, TKey> keySelector,
            IComparer<TKey>? comparer = null
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.CreateOrderedEnumerable(keySelector, comparer, false);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.thenbydescending
        public IOrderedAsyncEnumerable<T> ThenByDescending<TKey>(
            Func<T, TKey> keySelector,
            IComparer<TKey>? comparer = null
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.CreateOrderedEnumerable(keySelector, comparer, true);
        }
    }

    extension(IAsyncEnumerable<int> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<int> SumAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                sum = checked(sum + item);

            return sum;
        }
    }

    extension(IAsyncEnumerable<int?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<int?> SumAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                if (item is { } value)
                    sum = checked(sum + value);

            return sum;
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<int> SumAsync(
            Func<T, int> selector,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            var sum = 0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                sum = checked(sum + selector(item));

            return sum;
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<int?> SumAsync(
            Func<T, int?> selector,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            var sum = 0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                if (selector(item) is { } value)
                    sum = checked(sum + value);

            return sum;
        }
    }

    extension(IAsyncEnumerable<long> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<long> SumAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0L;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                sum = checked(sum + item);

            return sum;
        }
    }

    extension(IAsyncEnumerable<long?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<long?> SumAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0L;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                if (item is { } value)
                    sum = checked(sum + value);

            return sum;
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<long> SumAsync(
            Func<T, long> selector,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            var sum = 0L;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                sum = checked(sum + selector(item));

            return sum;
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<long?> SumAsync(
            Func<T, long?> selector,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            var sum = 0L;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                if (selector(item) is { } value)
                    sum = checked(sum + value);

            return sum;
        }
    }

    extension(IAsyncEnumerable<float> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<float> SumAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0f;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                sum += item;

            return sum;
        }
    }

    extension(IAsyncEnumerable<float?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<float?> SumAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0f;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                if (item is { } value)
                    sum += value;

            return sum;
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<float> SumAsync(
            Func<T, float> selector,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            var sum = 0f;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                sum += selector(item);

            return sum;
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<float?> SumAsync(
            Func<T, float?> selector,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            var sum = 0f;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                if (selector(item) is { } value)
                    sum += value;

            return sum;
        }
    }

    extension(IAsyncEnumerable<double> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<double> SumAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0.0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                sum += item;

            return sum;
        }
    }

    extension(IAsyncEnumerable<double?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<double?> SumAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0.0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                if (item is { } value)
                    sum += value;

            return sum;
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<double> SumAsync(
            Func<T, double> selector,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            var sum = 0.0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                sum += selector(item);

            return sum;
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<double?> SumAsync(
            Func<T, double?> selector,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            var sum = 0.0;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                if (selector(item) is { } value)
                    sum += value;

            return sum;
        }
    }

    extension(IAsyncEnumerable<decimal> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<decimal> SumAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0m;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                sum += item;

            return sum;
        }
    }

    extension(IAsyncEnumerable<decimal?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<decimal?> SumAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0m;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                if (item is { } value)
                    sum += value;

            return sum;
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<decimal> SumAsync(
            Func<T, decimal> selector,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            var sum = 0m;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                sum += selector(item);

            return sum;
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sum
        public async ValueTask<decimal?> SumAsync(
            Func<T, decimal?> selector,
            CancellationToken cancellationToken = default
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            var sum = 0m;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                if (selector(item) is { } value)
                    sum += value;

            return sum;
        }
    }

    extension(IAsyncEnumerable<object?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.cast
        public IAsyncEnumerable<TResult> Cast<TResult>()
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return CastIterator<TResult>(source);
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.oftype
        public IAsyncEnumerable<TResult> OfType<TResult>()
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return OfTypeIterator<TResult>(source);
        }
    }

    private sealed class OrderedAsyncEnumerable<T>(
        IAsyncEnumerable<T> source,
        Func<IEnumerable<T>, IOrderedEnumerable<T>> sorter
    ) : IOrderedAsyncEnumerable<T>
    {
        public IOrderedAsyncEnumerable<T> CreateOrderedEnumerable<TKey>(
            Func<T, TKey> keySelector,
            IComparer<TKey>? comparer,
            bool descending
        ) =>
            new OrderedAsyncEnumerable<T>(
                source,
                items => sorter(items).CreateOrderedEnumerable(keySelector, comparer, descending)
            );

        public async IAsyncEnumerator<T> GetAsyncEnumerator(
            CancellationToken cancellationToken = default
        )
        {
            var items = await source.ToListAsync(cancellationToken).ConfigureAwait(false);
            foreach (var item in sorter(items))
                yield return item;
        }
    }

    private static async IAsyncEnumerable<IGrouping<TKey, T>> GroupByIterator<T, TKey>(
        IAsyncEnumerable<T> source,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey>? comparer,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var items = await source.ToListAsync(cancellationToken).ConfigureAwait(false);
        foreach (var group in items.GroupBy(keySelector, comparer))
            yield return group;
    }

    private static async IAsyncEnumerable<T> ExceptIterator<T>(
        IAsyncEnumerable<T> first,
        IAsyncEnumerable<T> second,
        IEqualityComparer<T>? comparer,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var seen = await second.ToHashSetAsync(comparer, cancellationToken).ConfigureAwait(false);
        await foreach (var item in first.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (seen.Add(item))
                yield return item;
        }
    }

    private static async IAsyncEnumerable<T> UnionIterator<T>(
        IAsyncEnumerable<T> first,
        IAsyncEnumerable<T> second,
        IEqualityComparer<T>? comparer,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var seen = new HashSet<T>(comparer);
        await foreach (var item in first.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (seen.Add(item))
                yield return item;
        }

        await foreach (var item in second.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (seen.Add(item))
                yield return item;
        }
    }

    private static async IAsyncEnumerable<TResult> ZipIterator<TFirst, TSecond, TResult>(
        IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, TSecond, TResult> resultSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        await using var firstEnumerator = first.GetAsyncEnumerator(cancellationToken);
        await using var secondEnumerator = second.GetAsyncEnumerator(cancellationToken);

        while (
            await firstEnumerator.MoveNextAsync().ConfigureAwait(false)
            && await secondEnumerator.MoveNextAsync().ConfigureAwait(false)
        )
            yield return resultSelector(firstEnumerator.Current, secondEnumerator.Current);
    }

    private static async IAsyncEnumerable<TResult> JoinIterator<TOuter, TInner, TKey, TResult>(
        IAsyncEnumerable<TOuter> outer,
        IAsyncEnumerable<TInner> inner,
        Func<TOuter, TKey> outerKeySelector,
        Func<TInner, TKey> innerKeySelector,
        Func<TOuter, TInner, TResult> resultSelector,
        IEqualityComparer<TKey>? comparer,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var innerItems = await inner.ToListAsync(cancellationToken).ConfigureAwait(false);
        var lookup = innerItems.ToLookup(innerKeySelector, comparer);

        await foreach (var o in outer.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            var key = outerKeySelector(o);
            if (key is null)
                continue;

            foreach (var i in lookup[key])
                yield return resultSelector(o, i);
        }
    }

    private static async IAsyncEnumerable<T> EmptyIterator<T>()
    {
        await Task.CompletedTask.ConfigureAwait(false);
        yield break;
    }

    private static async IAsyncEnumerable<int> RangeIterator(int start, int count)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        for (var i = 0; i < count; i++)
            yield return start + i;
    }

    private static async IAsyncEnumerable<T> RepeatIterator<T>(T element, int count)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        for (var i = 0; i < count; i++)
            yield return element;
    }

    private static async IAsyncEnumerable<T> ToAsyncEnumerableIterator<T>(IEnumerable<T> source)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        foreach (var item in source)
            yield return item;
    }

    private static async IAsyncEnumerable<T> AppendIterator<T>(
        IAsyncEnumerable<T> source,
        T element,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            yield return item;

        yield return element;
    }

    private static async IAsyncEnumerable<T> PrependIterator<T>(
        IAsyncEnumerable<T> source,
        T element,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        yield return element;

        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            yield return item;
    }

    private static async IAsyncEnumerable<T> ConcatIterator<T>(
        IAsyncEnumerable<T> first,
        IAsyncEnumerable<T> second,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        await foreach (var item in first.WithCancellation(cancellationToken).ConfigureAwait(false))
            yield return item;

        await foreach (var item in second.WithCancellation(cancellationToken).ConfigureAwait(false))
            yield return item;
    }

    private static async IAsyncEnumerable<T?> DefaultIfEmptyIterator<T>(
        IAsyncEnumerable<T> source,
        T defaultValue,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var any = false;
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            any = true;
            yield return item;
        }

        if (!any)
            yield return defaultValue;
    }

    private static async IAsyncEnumerable<T> DistinctIterator<T>(
        IAsyncEnumerable<T> source,
        IEqualityComparer<T>? comparer,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var seen = new HashSet<T>(comparer);
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (seen.Add(item))
                yield return item;
        }
    }

    private static async IAsyncEnumerable<T> ReverseIterator<T>(
        IAsyncEnumerable<T> source,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var items = await source.ToListAsync(cancellationToken).ConfigureAwait(false);
        for (var i = items.Count - 1; i >= 0; i--)
            yield return items[i];
    }

    private static async IAsyncEnumerable<TResult> SelectIterator<T, TResult>(
        IAsyncEnumerable<T> source,
        Func<T, TResult> selector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            yield return selector(item);
    }

    private static async IAsyncEnumerable<TResult> SelectIndexedIterator<T, TResult>(
        IAsyncEnumerable<T> source,
        Func<T, int, TResult> selector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var index = 0;
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            yield return selector(item, checked(index++));
    }

    private static async IAsyncEnumerable<TResult> SelectAsyncIterator<T, TResult>(
        IAsyncEnumerable<T> source,
        Func<T, CancellationToken, ValueTask<TResult>> selector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            yield return await selector(item, cancellationToken).ConfigureAwait(false);
    }

    private static async IAsyncEnumerable<TResult> SelectManyIterator<T, TResult>(
        IAsyncEnumerable<T> source,
        Func<T, IAsyncEnumerable<TResult>> selector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            await foreach (
                var inner in selector(item)
                    .WithCancellation(cancellationToken)
                    .ConfigureAwait(false)
            )
                yield return inner;
        }
    }

    private static async IAsyncEnumerable<T> SkipIterator<T>(
        IAsyncEnumerable<T> source,
        int count,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var skipped = 0;
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (skipped < count)
                skipped++;
            else
                yield return item;
        }
    }

    private static async IAsyncEnumerable<T> SkipWhileIterator<T>(
        IAsyncEnumerable<T> source,
        Func<T, bool> predicate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var skipping = true;
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (skipping && predicate(item))
                continue;

            skipping = false;
            yield return item;
        }
    }

    private static async IAsyncEnumerable<T> TakeIterator<T>(
        IAsyncEnumerable<T> source,
        int count,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        if (count <= 0)
            yield break;

        var taken = 0;
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return item;

            if (++taken >= count)
                yield break;
        }
    }

    private static async IAsyncEnumerable<T> TakeWhileIterator<T>(
        IAsyncEnumerable<T> source,
        Func<T, bool> predicate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (!predicate(item))
                yield break;

            yield return item;
        }
    }

    private static async IAsyncEnumerable<T> WhereIterator<T>(
        IAsyncEnumerable<T> source,
        Func<T, bool> predicate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (predicate(item))
                yield return item;
        }
    }

    private static async IAsyncEnumerable<T> WhereIndexedIterator<T>(
        IAsyncEnumerable<T> source,
        Func<T, int, bool> predicate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var index = 0;
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (predicate(item, checked(index++)))
                yield return item;
        }
    }

    private static async IAsyncEnumerable<T> WhereAsyncIterator<T>(
        IAsyncEnumerable<T> source,
        Func<T, CancellationToken, ValueTask<bool>> predicate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (await predicate(item, cancellationToken).ConfigureAwait(false))
                yield return item;
        }
    }

    private static async IAsyncEnumerable<TResult> CastIterator<TResult>(
        IAsyncEnumerable<object?> source,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            yield return (TResult)item!;
    }

    private static async IAsyncEnumerable<TResult> OfTypeIterator<TResult>(
        IAsyncEnumerable<object?> source,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (item is TResult result)
                yield return result;
        }
    }
}
#endif
#endif
