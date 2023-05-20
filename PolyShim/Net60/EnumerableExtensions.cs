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