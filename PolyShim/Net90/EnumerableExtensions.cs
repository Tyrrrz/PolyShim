#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;

namespace System.Linq;

internal static partial class PolyfillExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.index
    public static IEnumerable<(int index, T value)> Index<T>(this IEnumerable<T> source) =>
        source.Select((value, index) => (index, value));

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.countby
    public static IEnumerable<KeyValuePair<TKey, int>> CountBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer = null
    )
        where TKey : notnull
    {
        var counts = new Dictionary<TKey, int>(comparer);

        foreach (var item in source)
        {
            var key = keySelector(item);
            counts[key] = counts.TryGetValue(key, out var count) ? count + 1 : 1;
        }

        return counts;
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.aggregateby#system-linq-enumerable-aggregateby-3(system-collections-generic-ienumerable((-0))-system-func((-0-1))-system-func((-1-2))-system-func((-2-0-2))-system-collections-generic-iequalitycomparer((-1)))
    public static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<
        TSource,
        TKey,
        TAccumulate
    >(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TKey, TAccumulate> seedSelector,
        Func<TAccumulate, TSource, TAccumulate> accumulator,
        IEqualityComparer<TKey>? keyComparer = null
    )
        where TKey : notnull
    {
        var aggregates = new Dictionary<TKey, TAccumulate>(keyComparer);

        foreach (var item in source)
        {
            var key = keySelector(item);

            aggregates[key] = accumulator(
                aggregates.TryGetValue(key, out var aggregate) ? aggregate : seedSelector(key),
                item
            );
        }

        return aggregates;
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.aggregateby#system-linq-enumerable-aggregateby-3(system-collections-generic-ienumerable((-0))-system-func((-0-1))-2-system-func((-2-0-2))-system-collections-generic-iequalitycomparer((-1)))
    public static IEnumerable<KeyValuePair<TKey, TAccumulate>> AggregateBy<
        TSource,
        TKey,
        TAccumulate
    >(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        TAccumulate seed,
        Func<TAccumulate, TSource, TAccumulate> accumulator,
        IEqualityComparer<TKey>? keyComparer = null
    )
        where TKey : notnull =>
        source.AggregateBy(keySelector, _ => seed, accumulator, keyComparer);
}
#endif
