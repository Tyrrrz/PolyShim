#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// Task, ValueTask, and async enumerable types are polyfilled when unavailable on a target framework
#if FEATURE_TASK
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
internal static class MemberPolyfills_Net110_AsyncEnumerable
{
    extension<TFirst>(IAsyncEnumerable<TFirst> first)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.fulljoin
        public IAsyncEnumerable<TResult> FullJoin<TSecond, TKey, TResult>(
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst?, TSecond?, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer = null
        )
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));
            if (second is null)
                throw new ArgumentNullException(nameof(second));
            if (firstKeySelector is null)
                throw new ArgumentNullException(nameof(firstKeySelector));
            if (secondKeySelector is null)
                throw new ArgumentNullException(nameof(secondKeySelector));
            if (resultSelector is null)
                throw new ArgumentNullException(nameof(resultSelector));

            return FullJoinIterator(
                first,
                second,
                firstKeySelector,
                secondKeySelector,
                resultSelector,
                comparer
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.fulljoin
        public IAsyncEnumerable<TResult> FullJoin<TSecond, TKey, TResult>(
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
            Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
            Func<TFirst?, TSecond?, CancellationToken, ValueTask<TResult>> resultSelector,
            IEqualityComparer<TKey>? comparer = null
        )
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));
            if (second is null)
                throw new ArgumentNullException(nameof(second));
            if (firstKeySelector is null)
                throw new ArgumentNullException(nameof(firstKeySelector));
            if (secondKeySelector is null)
                throw new ArgumentNullException(nameof(secondKeySelector));
            if (resultSelector is null)
                throw new ArgumentNullException(nameof(resultSelector));

            return FullJoinAsyncIterator(
                first,
                second,
                firstKeySelector,
                secondKeySelector,
                resultSelector,
                comparer
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.fulljoin
        public IAsyncEnumerable<(TFirst? First, TSecond? Second)> FullJoin<TSecond, TKey>(
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            IEqualityComparer<TKey>? comparer = null
        )
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));
            if (second is null)
                throw new ArgumentNullException(nameof(second));
            if (firstKeySelector is null)
                throw new ArgumentNullException(nameof(firstKeySelector));
            if (secondKeySelector is null)
                throw new ArgumentNullException(nameof(secondKeySelector));

            return FullJoinIterator(
                first,
                second,
                firstKeySelector,
                secondKeySelector,
                static (firstItem, secondItem) => (firstItem, secondItem),
                comparer
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.fulljoin
        public IAsyncEnumerable<(TFirst? First, TSecond? Second)> FullJoin<TSecond, TKey>(
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
            Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
            IEqualityComparer<TKey>? comparer = null
        )
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));
            if (second is null)
                throw new ArgumentNullException(nameof(second));
            if (firstKeySelector is null)
                throw new ArgumentNullException(nameof(firstKeySelector));
            if (secondKeySelector is null)
                throw new ArgumentNullException(nameof(secondKeySelector));

            return FullJoinAsyncIterator(
                first,
                second,
                firstKeySelector,
                secondKeySelector,
                static (firstItem, secondItem, _) =>
                    new ValueTask<(TFirst?, TSecond?)>((firstItem, secondItem)),
                comparer
            );
        }
    }

    private static async IAsyncEnumerable<TResult> FullJoinIterator<TFirst, TSecond, TKey, TResult>(
        IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, TKey> firstKeySelector,
        Func<TSecond, TKey> secondKeySelector,
        Func<TFirst?, TSecond?, TResult> resultSelector,
        IEqualityComparer<TKey>? comparer,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        if (ReferenceEquals(first, AsyncEnumerable.Empty<TFirst>()))
        {
            await foreach (
                var item in second.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                yield return resultSelector(default, item);

            yield break;
        }

        if (ReferenceEquals(second, AsyncEnumerable.Empty<TSecond>()))
        {
            await foreach (
                var item in first.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                yield return resultSelector(item, default);

            yield break;
        }

        var secondItems = await second.ToListAsync(cancellationToken).ConfigureAwait(false);
        var secondLookup = secondItems.ToLookup(secondKeySelector, comparer);
        var matchedGroups = new HashSet<IGrouping<TKey, TSecond>>();

        await foreach (
            var firstItem in first.WithCancellation(cancellationToken).ConfigureAwait(false)
        )
        {
            var key = firstKeySelector(firstItem);
            var group = key is null ? null : secondLookup[key] as IGrouping<TKey, TSecond>;
            if (group is null)
            {
                yield return resultSelector(firstItem, default);
                continue;
            }

            matchedGroups.Add(group);

            foreach (var secondItem in group)
                yield return resultSelector(firstItem, secondItem);
        }

        foreach (var group in secondLookup)
        {
            if (matchedGroups.Contains(group))
                continue;

            foreach (var secondItem in group)
                yield return resultSelector(default, secondItem);
        }
    }

    private static async IAsyncEnumerable<TResult> FullJoinAsyncIterator<
        TFirst,
        TSecond,
        TKey,
        TResult
    >(
        IAsyncEnumerable<TFirst> first,
        IAsyncEnumerable<TSecond> second,
        Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
        Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
        Func<TFirst?, TSecond?, CancellationToken, ValueTask<TResult>> resultSelector,
        IEqualityComparer<TKey>? comparer,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        if (ReferenceEquals(first, AsyncEnumerable.Empty<TFirst>()))
        {
            await foreach (
                var item in second.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                yield return await resultSelector(default, item, cancellationToken)
                    .ConfigureAwait(false);

            yield break;
        }

        if (ReferenceEquals(second, AsyncEnumerable.Empty<TSecond>()))
        {
            await foreach (
                var item in first.WithCancellation(cancellationToken).ConfigureAwait(false)
            )
                yield return await resultSelector(item, default, cancellationToken)
                    .ConfigureAwait(false);

            yield break;
        }

        var keyedSecondItems = new List<(TKey Key, TSecond Item)>();
        await foreach (var item in second.WithCancellation(cancellationToken).ConfigureAwait(false))
            keyedSecondItems.Add(
                (await secondKeySelector(item, cancellationToken).ConfigureAwait(false), item)
            );

        var secondLookup = keyedSecondItems.ToLookup(item => item.Key, item => item.Item, comparer);
        var matchedGroups = new HashSet<IGrouping<TKey, TSecond>>();

        await foreach (
            var firstItem in first.WithCancellation(cancellationToken).ConfigureAwait(false)
        )
        {
            var key = await firstKeySelector(firstItem, cancellationToken).ConfigureAwait(false);
            var group = key is null ? null : secondLookup[key] as IGrouping<TKey, TSecond>;
            if (group is null)
            {
                yield return await resultSelector(firstItem, default, cancellationToken)
                    .ConfigureAwait(false);
                continue;
            }

            matchedGroups.Add(group);

            foreach (var secondItem in group)
                yield return await resultSelector(firstItem, secondItem, cancellationToken)
                    .ConfigureAwait(false);
        }

        foreach (var group in secondLookup)
        {
            if (matchedGroups.Contains(group))
                continue;

            foreach (var secondItem in group)
                yield return await resultSelector(default, secondItem, cancellationToken)
                    .ConfigureAwait(false);
        }
    }
}
#endif
#endif
