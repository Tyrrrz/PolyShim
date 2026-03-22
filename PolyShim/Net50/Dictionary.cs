#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
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
internal static class MemberPolyfills_Net50_Dictionary
{
    extension<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2.ensurecapacity
        public int EnsureCapacity(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            // Dictionary<TKey,TValue> does not expose a Capacity property prior to .NET 5.
            // We return the requested capacity as a best-effort indication; the actual
            // internal capacity may differ, but it will grow automatically as needed.
            return capacity;
        }
    }
}
#endif
