#if (NETFRAMEWORK && !NET471_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_6_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.prepend
        public IEnumerable<T> Prepend(T element)
        {
            yield return element;

            foreach (var item in source)
                yield return item;
        }

        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.append
        public IEnumerable<T> Append(T element)
        {
            foreach (var item in source)
                yield return item;

            yield return element;
        }

#if (NETFRAMEWORK && !NET40_OR_GREATER)
        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.zip#system-linq-enumerable-zip-3(system-collections-generic-ienumerable((-0))-system-collections-generic-ienumerable((-1))-system-func((-0-1-2)))
        public IEnumerable<TResult> Zip<TOther, TResult>(
            IEnumerable<TOther> second,
            Func<T, TOther, TResult> resultSelector
        )
        {
            using var e1 = source.GetEnumerator();
            using var e2 = second.GetEnumerator();

            while (e1.MoveNext() && e2.MoveNext())
                yield return resultSelector(e1.Current, e2.Current);
        }
#endif
    }
}
#endif
