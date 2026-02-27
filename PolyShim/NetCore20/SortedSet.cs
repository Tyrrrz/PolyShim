#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK && !NET472_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#if !NETFRAMEWORK || NET40_OR_GREATER
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_SortedSet
{
    extension<T>(SortedSet<T> set)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.sortedset-1.trygetvalue
        public bool TryGetValue(T equalValue, out T? actualValue)
        {
            if (set.Contains(equalValue))
            {
                foreach (var item in set)
                {
                    if (set.Comparer.Compare(item, equalValue) == 0)
                    {
                        actualValue = item;
                        return true;
                    }
                }
            }

            actualValue = default;
            return false;
        }
    }
}
#endif
#endif
