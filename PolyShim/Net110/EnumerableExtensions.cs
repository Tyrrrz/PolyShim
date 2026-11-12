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

            IEnumerable<TResult> Iterate()
            {
                // Same as native: an empty first array streams the second sequence as-is,
                // without grouping it (which would reorder it) or invoking its key selector
                if (first is TFirst[] { Length: 0 })
                {
                    foreach (var secondItem in second)
                        yield return resultSelector(default, secondItem);

                    yield break;
                }

                var secondLookup = second.ToLookup(secondKeySelector, comparer);

                // Track matched groups (not keys), so memory is bounded by the number of
                // second-sequence groups regardless of how many distinct first keys there are.
                var matchedGroups = new HashSet<IGrouping<TKey, TSecond>>();

                foreach (var firstItem in first)
                {
                    var key = firstKeySelector(firstItem);

                    // Null keys never participate in matches
                    var group = key is null ? null : secondLookup[key] as IGrouping<TKey, TSecond>;
                    if (group is null)
                    {
                        yield return resultSelector(firstItem, default);
                        continue;
                    }

                    matchedGroups.Add(group);

                    foreach (var secondItem in group)
                        yield return resultSelector(firstItem, secondItem);
                }

                foreach (var group in secondLookup)
                {
                    if (matchedGroups.Contains(group))
                        continue;

                    foreach (var secondItem in group)
                        yield return resultSelector(default, secondItem);
                }
            }

            return Iterate();
        }

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
    }
}
#endif
