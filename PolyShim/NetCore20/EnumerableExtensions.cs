#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore20_EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.takelast
        public IEnumerable<T> TakeLast(int count) => source.Reverse().Take(count).Reverse();

        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.skiplast
        public IEnumerable<T> SkipLast(int count) => source.Reverse().Skip(count).Reverse();

#if !NET472_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.tohashset#system-linq-enumerable-tohashset-1(system-collections-generic-ienumerable((-0))-system-collections-generic-iequalitycomparer((-0)))
        public HashSet<T> ToHashSet(IEqualityComparer<T> comparer) => new(source, comparer);

        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.tohashset#system-linq-enumerable-tohashset-1(system-collections-generic-ienumerable((-0)))
        public HashSet<T> ToHashSet() => source.ToHashSet(EqualityComparer<T>.Default);
#endif
    }
}
#endif
