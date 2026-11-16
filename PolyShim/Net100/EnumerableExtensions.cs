#if (NETCOREAPP && !NET10_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net100_EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.shuffle
        public IEnumerable<T> Shuffle()
        {
            ArgumentNullException.ThrowIfNull(source);

            IEnumerable<T> Iterate()
            {
                var items = source.ToArray();
                Random.Shared.Shuffle(items);

                foreach (var item in items)
                    yield return item;
            }

            return Iterate();
        }
    }
}
#endif
