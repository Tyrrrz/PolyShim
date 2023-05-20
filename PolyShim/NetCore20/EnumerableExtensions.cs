#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;

namespace System.Linq;

internal static partial class PolyfillExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.takelast
    public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count) =>
        source.Reverse().Take(count).Reverse();

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.skiplast
    public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count) =>
        source.Reverse().Skip(count).Reverse();

#if !NET472_OR_GREATER
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.tohashset#system-linq-enumerable-tohashset-1(system-collections-generic-ienumerable((-0))-system-collections-generic-iequalitycomparer((-0)))
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer) =>
        new(source, comparer);

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.tohashset#system-linq-enumerable-tohashset-1(system-collections-generic-ienumerable((-0)))
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source) =>
        source.ToHashSet(EqualityComparer<T>.Default);
#endif
}
#endif