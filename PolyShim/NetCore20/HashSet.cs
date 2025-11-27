#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK && !NET472_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;

internal static partial class PolyfillExtensions
{
    extension<T>(HashSet<T> set)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.hashset-1.trygetvalue
        public bool TryGetValue(T equalValue, out T? actualValue)
        {
            // HashSet uses its comparer to find the element
            // We need to iterate through the set to find the actual stored value
            foreach (var item in set)
            {
                if (set.Comparer.Equals(item, equalValue))
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
