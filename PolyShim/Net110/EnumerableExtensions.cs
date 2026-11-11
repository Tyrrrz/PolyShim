#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net110_EnumerableExtensions
{
    extension<TFirst>(IEnumerable<TFirst> first)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.fulljoin
        public IEnumerable<(TFirst? Outer, TInner? Inner)> FullJoin<TInner, TKey>(
            IEnumerable<TInner> inner,
            Func<TFirst, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            IEqualityComparer<TKey>? comparer = null
        ) =>
            first.FullJoin(
                inner,
                outerKeySelector,
                innerKeySelector,
                (outerItem, innerItem) => (outerItem, innerItem),
                comparer
            );

        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.fulljoin
        public IEnumerable<TResult> FullJoin<TSecond, TKey, TResult>(
            IEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst?, TSecond?, TResult> resultSelector,
            IEqualityComparer<TKey>? comparer = null
        )
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));
            if (second is null)
                throw new ArgumentNullException(nameof(second));
            if (firstKeySelector is null)
                throw new ArgumentNullException(nameof(firstKeySelector));
            if (secondKeySelector is null)
                throw new ArgumentNullException(nameof(secondKeySelector));
            if (resultSelector is null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Iterate();

            IEnumerable<TResult> Iterate()
            {
                var secondLookup = second.ToLookup(secondKeySelector, comparer);
                var matchedKeys = new HashSet<TKey>(comparer!);
                var hasNullKey = false;

                foreach (var firstItem in first)
                {
                    var key = firstKeySelector(firstItem);

                    if (key is null)
                        hasNullKey = true;
                    else
                        matchedKeys.Add(key);

                    var matches = secondLookup[key];
                    var any = false;

                    foreach (var secondItem in matches)
                    {
                        any = true;
                        yield return resultSelector(firstItem, secondItem);
                    }

                    if (!any)
                        yield return resultSelector(firstItem, default);
                }

                foreach (var group in secondLookup)
                {
                    if (group.Key is null ? hasNullKey : matchedKeys.Contains(group.Key))
                        continue;

                    foreach (var secondItem in group)
                        yield return resultSelector(default, secondItem);
                }
            }
        }
    }
}
#endif
