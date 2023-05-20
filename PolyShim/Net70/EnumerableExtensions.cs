﻿#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;

namespace System.Linq;

internal static partial class PolyfillExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.order#system-linq-enumerable-order-1(system-collections-generic-ienumerable((-0))-system-collections-generic-icomparer((-0)))
    public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> source, IComparer<T>? comparer) =>
        source.OrderBy(x => x, comparer);

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.order#system-linq-enumerable-order-1(system-collections-generic-ienumerable((-0)))
    public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> source) =>
        source.Order(Comparer<T>.Default);

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.orderdescending#system-linq-enumerable-orderdescending-1(system-collections-generic-ienumerable((-0))-system-collections-generic-icomparer((-0)))
    public static IOrderedEnumerable<T> OrderDescending<T>(this IEnumerable<T> source, IComparer<T>? comparer) =>
        source.OrderByDescending(x => x, comparer);

    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.orderdescending#system-linq-enumerable-orderdescending-1(system-collections-generic-ienumerable((-0)))
    public static IOrderedEnumerable<T> OrderDescending<T>(this IEnumerable<T> source) =>
        source.OrderDescending(Comparer<T>.Default);
}
#endif