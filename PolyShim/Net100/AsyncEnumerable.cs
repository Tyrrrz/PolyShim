#if (NETCOREAPP && !NET10_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#if FEATURE_TASK && !FEATURE_LINQ_ASYNCENUMERABLE
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
    public static IAsyncEnumerable<T> Empty<T>() => EmptyCache<T>.Instance;

    // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.range
    public static IAsyncEnumerable<int> Range(int start, int count)
    {
        if (count < 0 || (long)start + count - 1 > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(count));

        return Iterate();

        async IAsyncEnumerable<int> Iterate(
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            for (var i = 0; i < count; i++)
                yield return start + i;
        }
    }

    // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.repeat
    public static IAsyncEnumerable<T> Repeat<T>(T element, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        return Iterate();

        async IAsyncEnumerable<T> Iterate(
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            for (var i = 0; i < count; i++)
                yield return element;
        }
    }

    extension<T>(IEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.toasyncenumerable
        public IAsyncEnumerable<T> ToAsyncEnumerable()
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return Iterate();

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                foreach (var item in source)
                    yield return item;
            }
        }
    }

    extension<T>(IAsyncEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.aggregateasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.aggregateasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.allasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.allasync
        public async ValueTask<bool> AllAsync(
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.anyasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.anyasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.anyasync
        public async ValueTask<bool> AnyAsync(
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

            return Iterate();

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
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.prepend
        public IAsyncEnumerable<T> Prepend(T element)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return Iterate();

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
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.concat
        public IAsyncEnumerable<T> Concat(IAsyncEnumerable<T> second)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (second is null)
                throw new ArgumentNullException(nameof(second));

            return Iterate();

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                    yield return item;

                await foreach (
                    var item in second.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                    yield return item;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.containsasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.countasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.countasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.countasync
        public async ValueTask<int> CountAsync(
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.longcountasync
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

            return Iterate();

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
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.distinct
        public IAsyncEnumerable<T> Distinct(IEqualityComparer<T>? comparer = null)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return Iterate();

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
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.elementatasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.elementatordefaultasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstordefaultasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.firstordefaultasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.lastasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.lastordefaultasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.singleasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.singleordefaultasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.maxasync
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
                if (item is null)
                    continue;

                if (!hasValue || comparer.Compare(item, result!) > 0)
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
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            comparer ??= Comparer<T>.Default;
            var hasValue = false;
            var result = default(T);
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
            {
                if (item is null)
                    continue;

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

            return Iterate();

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var items = await source.ToListAsync(cancellationToken).ConfigureAwait(false);
                for (var i = items.Count - 1; i >= 0; i--)
                    yield return items[i];
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.select
        public IAsyncEnumerable<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return Iterate();

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                    yield return selector(item);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.select
        public IAsyncEnumerable<TResult> Select<TResult>(Func<T, int, TResult> selector)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return Iterate();

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var index = 0;
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                    yield return selector(item, checked(index++));
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.selectasync
        public IAsyncEnumerable<TResult> Select<TResult>(
            Func<T, CancellationToken, ValueTask<TResult>> selector
        )
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return Iterate();

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                    yield return await selector(item, cancellationToken).ConfigureAwait(false);
            }
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

            return Iterate();

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    await foreach (
                        var inner in selector(item)
                            .WithCancellation(cancellationToken)
                            .ConfigureAwait(false)
                    )
                        yield return inner;
                }
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sequenceequalasync
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

            return Iterate();

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
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.skipwhile
        public IAsyncEnumerable<T> SkipWhile(Func<T, bool> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return Iterate();

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var skipping = true;
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (skipping && predicate(item))
                        continue;

                    skipping = false;
                    yield return item;
                }
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.take
        public IAsyncEnumerable<T> Take(int count)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return Iterate();

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
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.takewhile
        public IAsyncEnumerable<T> TakeWhile(Func<T, bool> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return Iterate();

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (!predicate(item))
                        yield break;

                    yield return item;
                }
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.where
        public IAsyncEnumerable<T> Where(Func<T, bool> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return Iterate();

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (predicate(item))
                        yield return item;
                }
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.where
        public IAsyncEnumerable<T> Where(Func<T, int, bool> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return Iterate();

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var index = 0;
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (predicate(item, checked(index++)))
                        yield return item;
                }
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.whereasync
        public IAsyncEnumerable<T> Where(Func<T, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return Iterate();

            async IAsyncEnumerable<T> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    if (await predicate(item, cancellationToken).ConfigureAwait(false))
                        yield return item;
                }
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.toarrayasync
        public async ValueTask<T[]> ToArrayAsync(CancellationToken cancellationToken = default) =>
            (await source.ToListAsync(cancellationToken).ConfigureAwait(false)).ToArray();

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.todictionaryasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.tohashsetasync
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

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.tolistasync
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
                (items, _) =>
                    new ValueTask<IOrderedEnumerable<T>>(items.OrderBy(keySelector, comparer))
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
                (items, _) =>
                    new ValueTask<IOrderedEnumerable<T>>(
                        items.OrderByDescending(keySelector, comparer)
                    )
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

            return Iterate();

            async IAsyncEnumerable<IGrouping<TKey, T>> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var items = await source.ToListAsync(cancellationToken).ConfigureAwait(false);
                foreach (var group in items.GroupBy(keySelector, comparer))
                    yield return group;
            }
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

            return Iterate();

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

            return Iterate();

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

            return Iterate();

            async IAsyncEnumerable<(T First, TSecond Second)> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await using var firstEnumerator = source.GetAsyncEnumerator(cancellationToken);
                await using var secondEnumerator = second.GetAsyncEnumerator(cancellationToken);

                while (
                    await firstEnumerator.MoveNextAsync().ConfigureAwait(false)
                    && await secondEnumerator.MoveNextAsync().ConfigureAwait(false)
                )
                    yield return (firstEnumerator.Current, secondEnumerator.Current);
            }
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

            return Iterate();

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var innerItems = await inner.ToListAsync(cancellationToken).ConfigureAwait(false);
                var lookup = innerItems.ToLookup(innerKeySelector, comparer);

                await foreach (
                    var outerItem in outer.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                {
                    var key = outerKeySelector(outerItem);
                    if (key is null)
                        continue;

                    foreach (var innerItem in lookup[key])
                        yield return resultSelector(outerItem, innerItem);
                }
            }
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

            return Iterate();

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await using var firstEnumerator = outer.GetAsyncEnumerator(cancellationToken);
                await using var secondEnumerator = second.GetAsyncEnumerator(cancellationToken);

                while (
                    await firstEnumerator.MoveNextAsync().ConfigureAwait(false)
                    && await secondEnumerator.MoveNextAsync().ConfigureAwait(false)
                )
                    yield return resultSelector(firstEnumerator.Current, secondEnumerator.Current);
            }
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

            return source.CreateOrderedAsyncEnumerable(keySelector, comparer, false);
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

            return source.CreateOrderedAsyncEnumerable(keySelector, comparer, true);
        }
    }

    extension(IAsyncEnumerable<object?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.cast
        public IAsyncEnumerable<TResult> Cast<TResult>()
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return Iterate();

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await foreach (
                    var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
                    yield return (TResult)item!;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.oftype
        public IAsyncEnumerable<TResult> OfType<TResult>()
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return Iterate();

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
        }
    }

    extension(IAsyncEnumerable<int> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync
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
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync
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

    extension(IAsyncEnumerable<long> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync
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
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync
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

    extension(IAsyncEnumerable<float> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync
        public async ValueTask<float> SumAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0d;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                sum += item;

            return (float)sum;
        }
    }

    extension(IAsyncEnumerable<float?> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync
        public async ValueTask<float?> SumAsync(CancellationToken cancellationToken = default)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0d;
            await foreach (
                var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                if (item is { } value)
                    sum += value;

            return (float)sum;
        }
    }

    extension(IAsyncEnumerable<double> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync
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
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync
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

    extension(IAsyncEnumerable<decimal> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync
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
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.sumasync
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

    private static class EmptyCache<T>
    {
        internal static readonly IAsyncEnumerable<T> Instance = CreateInstance();

        private static IAsyncEnumerable<T> CreateInstance()
        {
            async IAsyncEnumerable<T> Iterate()
            {
                await Task.CompletedTask.ConfigureAwait(false);
                yield break;
            }

            return Iterate();
        }
    }
}
#endif
#endif
