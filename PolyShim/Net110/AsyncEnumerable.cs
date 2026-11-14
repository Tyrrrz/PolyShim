#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// Task and ValueTask are not available on all target frameworks within this TFM range without a NuGet package reference
#if FEATURE_TASK && FEATURE_VALUETASK
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net110_AsyncEnumerable
{
    extension<TFirst>(IAsyncEnumerable<TFirst> first)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.asyncenumerable.fulljoin
        public IAsyncEnumerable<TResult> FullJoin<TSecond, TKey, TResult>(
            IAsyncEnumerable<TSecond> second,
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

            async IAsyncEnumerable<TResult> Iterate(
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            {
                var secondItems = await second.ToListAsync(cancellationToken).ConfigureAwait(false);
                var secondLookup = secondItems.ToLookup(secondKeySelector, comparer);
                var matchedGroups = new HashSet<IGrouping<TKey, TSecond>>();

                await foreach (
                    var firstItem in first.WithCancellation(cancellationToken).ConfigureAwait(false)
                )
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
    }
}
#endif
#endif
