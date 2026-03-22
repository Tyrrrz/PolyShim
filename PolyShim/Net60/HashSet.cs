#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net60_HashSet
{
    extension<T>(HashSet<T> set)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.hashset-1.ensurecapacity
        public int EnsureCapacity(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            // Note: HashSet<T> does not expose a writable Capacity property prior to .NET 6.
            // This polyfill validates the argument and returns the requested value, but cannot
            // actually pre-allocate internal storage. The set will resize on demand as entries
            // are added. This is a best-effort, API-compatible stub.
            return capacity;
        }
    }
}
#endif
