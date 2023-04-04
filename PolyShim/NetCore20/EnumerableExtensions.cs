#if (NETCOREAPP1_0_OR_GREATER && !NETCOREAPP2_0_OR_GREATER) || (NET35_OR_GREATER && !NET472_OR_GREATER) || (NETSTANDARD1_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq;

[ExcludeFromCodeCoverage]
internal static class _9D1F83230CAD4E3FB0784CB484ADF520
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.tohashset#system-linq-enumerable-tohashset-1(system-collections-generic-ienumerable((-0))-system-collections-generic-iequalitycomparer((-0)))
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer) =>
        new(source, comparer);

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.tohashset#system-linq-enumerable-tohashset-1(system-collections-generic-ienumerable((-0)))
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source) =>
        source.ToHashSet(EqualityComparer<T>.Default);
}
#endif