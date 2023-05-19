#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq;

[ExcludeFromCodeCoverage]
internal static class _5F37AE3F29344B63A302CBE9D66BBA2D
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.minby#system-linq-enumerable-minby-2(system-collections-generic-ienumerable((-0))-system-func((-0-1))-system-collections-generic-icomparer((-1)))
    public static T? MinBy<T, TKey>(
        this IEnumerable<T> source,
        Func<T, TKey> keySelector,
        IComparer<TKey> comparer) =>
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
        IComparer<TKey> comparer) =>
        source.OrderByDescending(keySelector, comparer).FirstOrDefault();

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.maxby#system-linq-enumerable-maxby-2(system-collections-generic-ienumerable((-0))-system-func((-0-1)))
    public static T? MaxBy<T, TKey>(
        this IEnumerable<T> source,
        Func<T, TKey> keySelector) =>
        source.MaxBy(keySelector, Comparer<TKey>.Default);
}
#endif