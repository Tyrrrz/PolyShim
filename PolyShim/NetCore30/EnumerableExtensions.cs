#if (NETCOREAPP1_0_OR_GREATER && !NETCOREAPP3_0_OR_GREATER) || (NET20_OR_GREATER) || (NETSTANDARD1_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq;

[ExcludeFromCodeCoverage]
internal static class _B118719A088C4AAD9AFE93F23519FDAA
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.zip#system-linq-enumerable-zip-2(system-collections-generic-ienumerable((-0))-system-collections-generic-ienumerable((-1)))
    public static IEnumerable<(TFirst left, TSecond right)> Zip<TFirst, TSecond>(
        this IEnumerable<TFirst> first,
        IEnumerable<TSecond> second) =>
        first.Zip(second, (x, y) => (x, y));
}
#endif