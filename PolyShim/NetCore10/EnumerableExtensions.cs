#if (NET20_OR_GREATER && !NET471_OR_GREATER) || (NETSTANDARD1_0_OR_GREATER && !NETSTANDARD1_6_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq;

[ExcludeFromCodeCoverage]
internal static class _25ACEBAC35F840B295CD58760791CBF4
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.prepend
    public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T item)
    {
        yield return item;

        foreach (var i in source)
            yield return i;
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.append
    public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
    {
        foreach (var i in source)
            yield return i;

        yield return item;
    }

#if (NET20_OR_GREATER && !NET40_OR_GREATER)
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.zip#system-linq-enumerable-zip-3(system-collections-generic-ienumerable((-0))-system-collections-generic-ienumerable((-1))-system-func((-0-1-2)))
    public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
        this IEnumerable<TFirst> first,
        IEnumerable<TSecond> second,
        Func<TFirst, TSecond, TResult> resultSelector)
    {
        using var e1 = first.GetEnumerator();
        using var e2 = second.GetEnumerator();

        while (e1.MoveNext() && e2.MoveNext())
            yield return resultSelector(e1.Current, e2.Current);
    }
#endif
}
#endif