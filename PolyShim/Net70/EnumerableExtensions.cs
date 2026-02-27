#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

namespace System.Linq;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.order#system-linq-enumerable-order-1(system-collections-generic-ienumerable((-0))-system-collections-generic-icomparer((-0)))
        public IOrderedEnumerable<T> Order(IComparer<T>? comparer) =>
            source.OrderBy(x => x, comparer);

        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.order#system-linq-enumerable-order-1(system-collections-generic-ienumerable((-0)))
        public IOrderedEnumerable<T> Order() => source.Order(Comparer<T>.Default);

        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.orderdescending#system-linq-enumerable-orderdescending-1(system-collections-generic-ienumerable((-0))-system-collections-generic-icomparer((-0)))
        public IOrderedEnumerable<T> OrderDescending(IComparer<T>? comparer) =>
            source.OrderByDescending(x => x, comparer);

        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.orderdescending#system-linq-enumerable-orderdescending-1(system-collections-generic-ienumerable((-0)))
        public IOrderedEnumerable<T> OrderDescending() =>
            source.OrderDescending(Comparer<T>.Default);
    }
}
#endif
