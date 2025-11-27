#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK && !NET472_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

internal static partial class PolyfillExtensions
{
    extension<T>(SortedSet<T> set)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.sortedset-1.trygetvalue
        public bool TryGetValue(T equalValue, [MaybeNullWhen(false)] out T actualValue)
        {
            // SortedSet uses its comparer to find the element
            // We need to iterate through the set to find the actual stored value
            foreach (var item in set)
            {
                if (set.Comparer.Compare(item, equalValue) == 0)
                {
                    actualValue = item;
                    return true;
                }
            }

            actualValue = default;
            return false;
        }
    }
}
#endif
