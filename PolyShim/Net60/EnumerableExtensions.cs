#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;

namespace System.Linq;

internal static partial class PolyfillExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.minby#system-linq-enumerable-minby-2(system-collections-generic-ienumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))
    public static T? MinBy<T, TKey>(
        this IEnumerable<T> source,
        Func<T, TKey> keySelector,
        IComparer<TKey>? comparer) =>
        source.OrderBy(keySelector, comparer).FirstOrDefault();

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.minby#system-linq-enumerable-minby-2(system-collections-generic-ienumerable((-0))-system-func((-0-1)))
    public static T? MinBy<T, TKey>(
        this IEnumerable<T> source,
        Func<T, TKey> keySelector) =>
        source.MinBy(keySelector, Comparer<TKey>.Default);

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.maxby#system-linq-enumerable-maxby-2(system-collections-generic-ienumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))
    public static T? MaxBy<T, TKey>(
        this IEnumerable<T> source,
        Func<T, TKey> keySelector,
        IComparer<TKey>? comparer) =>
        source.OrderByDescending(keySelector, comparer).FirstOrDefault();

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.maxby#system-linq-enumerable-maxby-2(system-collections-generic-ienumerable((-0))-system-func((-0-1)))
    public static T? MaxBy<T, TKey>(
        this IEnumerable<T> source,
        Func<T, TKey> keySelector) =>
        source.MaxBy(keySelector, Comparer<TKey>.Default);

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.distinctby#system-linq-enumerable-distinctby-2(system-collections-generic-ienumerable((-0))-system-func((-0-1))-system-collections-generic-iequalitycomparer((-1)))
    public static IEnumerable<T> DistinctBy<T, TKey>(
        this IEnumerable<T> source,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey>? comparer) =>
        source.GroupBy(keySelector, comparer).Select(x => x.First());

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.distinctby#system-linq-enumerable-distinctby-2(system-collections-generic-ienumerable((-0))-system-func((-0-1)))
    public static IEnumerable<T> DistinctBy<T, TKey>(
        this IEnumerable<T> source,
        Func<T, TKey> keySelector) =>
        source.DistinctBy(keySelector, EqualityComparer<TKey>.Default);

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.exceptby#system-linq-enumerable-exceptby-2(system-collections-generic-ienumerable((-0))-system-collections-generic-ienumerable((-1))-system-func((-0-1))-system-collections-generic-iequalitycomparer((-1)))
    public static IEnumerable<T> ExceptBy<T, TKey>(
        this IEnumerable<T> source,
        IEnumerable<TKey> other,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        var set = new HashSet<TKey>(other, comparer);

        foreach (var item in source)
        {
            if (!set.Contains(keySelector(item)))
                yield return item;
        }
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.exceptby#system-linq-enumerable-exceptby-2(system-collections-generic-ienumerable((-0))-system-collections-generic-ienumerable((-1))-system-func((-0-1)))
    public static IEnumerable<T> ExceptBy<T, TKey>(
        this IEnumerable<T> source,
        IEnumerable<TKey> other,
        Func<T, TKey> keySelector) =>
        source.ExceptBy(other, keySelector, EqualityComparer<TKey>.Default);

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.intersectby#system-linq-enumerable-intersectby-2(system-collections-generic-ienumerable((-0))-system-collections-generic-ienumerable((-1))-system-func((-0-1))-system-collections-generic-iequalitycomparer((-1)))
    public static IEnumerable<T> IntersectBy<T, TKey>(
        this IEnumerable<T> source,
        IEnumerable<TKey> other,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        var set = new HashSet<TKey>(other, comparer);

        foreach (var item in source)
        {
            if (set.Contains(keySelector(item)))
                yield return item;
        }
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.unionby#system-linq-enumerable-unionby-2(system-collections-generic-ienumerable((-0))-system-collections-generic-ienumerable((-0))-system-func((-0-1))-system-collections-generic-iequalitycomparer((-1)))
    public static IEnumerable<T> UnionBy<T, TKey>(
        this IEnumerable<T> source,
        IEnumerable<T> other,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey>? comparer)
    {
        var set = new HashSet<TKey>(comparer);

        foreach (var item in source)
        {
            if (set.Add(keySelector(item)))
                yield return item;
        }

        foreach (var item in other)
        {
            if (set.Add(keySelector(item)))
                yield return item;
        }
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.unionby#system-linq-enumerable-unionby-2(system-collections-generic-ienumerable((-0))-system-collections-generic-ienumerable((-0))-system-func((-0-1)))
    public static IEnumerable<T> UnionBy<T, TKey>(
        this IEnumerable<T> source,
        IEnumerable<T> other,
        Func<T, TKey> keySelector) =>
        source.UnionBy(other, keySelector, EqualityComparer<TKey>.Default);

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.intersectby#system-linq-enumerable-intersectby-2(system-collections-generic-ienumerable((-0))-system-collections-generic-ienumerable((-1))-system-func((-0-1)))
    public static IEnumerable<T> IntersectBy<T, TKey>(
        this IEnumerable<T> source,
        IEnumerable<TKey> other,
        Func<T, TKey> keySelector) =>
        source.IntersectBy(other, keySelector, EqualityComparer<TKey>.Default);

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.chunk
    public static IEnumerable<T[]> Chunk<T>(this IEnumerable<T> source, int size)
    {
        var chunk = new List<T>(size);
        foreach (var item in source)
        {
            chunk.Add(item);
            if (chunk.Count == size)
            {
                yield return chunk.ToArray();
                chunk.Clear();
            }
        }

        if (chunk.Count > 0)
            yield return chunk.ToArray();
    }
}
#endif