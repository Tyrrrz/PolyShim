#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
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
internal static class MemberPolyfills_Net110_AsyncEnumerable
{
    extension<TFirst>(IAsyncEnumerable<TFirst> first)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.fulljoin?system-linq-asyncenumerable-fulljoin-4(system-collections-generic-iasyncenumerable((-0))-system-collections-generic-iasyncenumerable((-1))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-func((-1-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-func((-0-1-system-threading-cancellationtoken-system-threading-tasks-valuetask((-3))))-system-collections-generic-iequalitycomparer((-2)))
        public IAsyncEnumerable<TResult> FullJoin<TSecond, TKey, TResult>(
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
            Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
            Func<TFirst?, TSecond?, CancellationToken, ValueTask<TResult>> resultSelector,
            IEqualityComparer<TKey>? comparer = null
        )
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);
            ArgumentNullException.ThrowIfNull(firstKeySelector);
            ArgumentNullException.ThrowIfNull(secondKeySelector);
            ArgumentNullException.ThrowIfNull(resultSelector);

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                await using var firstEnumerator = first.GetAsyncEnumerator(cancellationToken);

                // No first items: every second item is unmatched (and its key is never computed)
                if (!await firstEnumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    await foreach (
                        var secondItem in second
                            .WithCancellation(cancellationToken)
                            .ConfigureAwait(false)
                    )
                    {
                        yield return await resultSelector(default, secondItem, cancellationToken)
                            .ConfigureAwait(false);
                    }

                    yield break;
                }

                var keyedSecondItems = new List<(TKey Key, TSecond Item)>();
                await foreach (
                    var secondItem in second
                        .WithCancellation(cancellationToken)
                        .ConfigureAwait(false)
                )
                {
                    var secondKey = await secondKeySelector(secondItem, cancellationToken)
                        .ConfigureAwait(false);

                    keyedSecondItems.Add((secondKey, secondItem));
                }

                // No second items: every first item is unmatched (and its key is never computed)
                if (keyedSecondItems.Count == 0)
                {
                    do
                    {
                        yield return await resultSelector(
                                firstEnumerator.Current,
                                default,
                                cancellationToken
                            )
                            .ConfigureAwait(false);
                    } while (await firstEnumerator.MoveNextAsync().ConfigureAwait(false));

                    yield break;
                }

                var lookup = keyedSecondItems.ToLookup(
                    item => item.Key,
                    item => item.Item,
                    comparer
                );

                var matchedKeys = new HashSet<TKey>(comparer);

                do
                {
                    var firstItem = firstEnumerator.Current;
                    var firstKey = await firstKeySelector(firstItem, cancellationToken)
                        .ConfigureAwait(false);

                    // Null keys never match
                    if (firstKey is null || !lookup.Contains(firstKey))
                    {
                        yield return await resultSelector(firstItem, default, cancellationToken)
                            .ConfigureAwait(false);

                        continue;
                    }

                    matchedKeys.Add(firstKey);

                    foreach (var matchedItem in lookup[firstKey])
                    {
                        yield return await resultSelector(firstItem, matchedItem, cancellationToken)
                            .ConfigureAwait(false);
                    }
                } while (await firstEnumerator.MoveNextAsync().ConfigureAwait(false));

                foreach (var group in lookup)
                {
                    if (group.Key is not null && matchedKeys.Contains(group.Key))
                        continue;

                    foreach (var unmatchedItem in group)
                    {
                        yield return await resultSelector(default, unmatchedItem, cancellationToken)
                            .ConfigureAwait(false);
                    }
                }
            }

            return Iterate();
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.fulljoin?system-linq-asyncenumerable-fulljoin-4(system-collections-generic-iasyncenumerable((-0))-system-collections-generic-iasyncenumerable((-1))-system-func((-0-2))-system-func((-1-2))-system-func((-0-1-3))-system-collections-generic-iequalitycomparer((-2)))
        public IAsyncEnumerable<TResult> FullJoin<TSecond, TKey, TResult>(
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst?, TSecond?, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer = null
        ) =>
            first.FullJoin(
                second,
                (firstItem, _) => new ValueTask<TKey>(firstKeySelector(firstItem)),
                (secondItem, _) => new ValueTask<TKey>(secondKeySelector(secondItem)),
                (firstItem, secondItem, _) =>
                    new ValueTask<TResult>(resultSelector(firstItem, secondItem)),
                comparer
            );

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.fulljoin?system-linq-asyncenumerable-fulljoin-3(system-collections-generic-iasyncenumerable((-0))-system-collections-generic-iasyncenumerable((-1))-system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-func((-1-system-threading-cancellationtoken-system-threading-tasks-valuetask((-2))))-system-collections-generic-iequalitycomparer((-2)))
        public IAsyncEnumerable<(TFirst? First, TSecond? Second)> FullJoin<TSecond, TKey>(
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, CancellationToken, ValueTask<TKey>> firstKeySelector,
            Func<TSecond, CancellationToken, ValueTask<TKey>> secondKeySelector,
            IEqualityComparer<TKey>? comparer = null
        ) =>
            first.FullJoin(
                second,
                firstKeySelector,
                secondKeySelector,
                (firstItem, secondItem, _) =>
                    new ValueTask<(TFirst? First, TSecond? Second)>((firstItem, secondItem)),
                comparer
            );

        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.fulljoin?system-linq-asyncenumerable-fulljoin-3(system-collections-generic-iasyncenumerable((-0))-system-collections-generic-iasyncenumerable((-1))-system-func((-0-2))-system-func((-1-2))-system-collections-generic-iequalitycomparer((-2)))
        public IAsyncEnumerable<(TFirst? First, TSecond? Second)> FullJoin<TSecond, TKey>(
            IAsyncEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            IEqualityComparer<TKey>? comparer = null
        ) =>
            first.FullJoin(
                second,
                (firstItem, _) => new ValueTask<TKey>(firstKeySelector(firstItem)),
                (secondItem, _) => new ValueTask<TKey>(secondKeySelector(secondItem)),
                comparer
            );
    }
}
#endif
#endif
