#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
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
internal static class MemberPolyfills_Net50_Dictionary
{
    extension<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2.ensurecapacity
        public int EnsureCapacity(int capacity)
        {
            // Note: Dictionary<TKey,TValue> does not expose a writable Capacity property prior
            // to .NET 5. This polyfill returns the requested value but cannot actually
            // pre-allocate internal storage. The dictionary will resize on demand as entries
            // are added. This is a best-effort, API-compatible stub.
            return capacity;
        }
    }
}
#endif
