#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

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
        // Note: Dictionary<TKey,TValue> does not expose a writable Capacity property prior
        // to .NET 5. This polyfill returns a value compatible with the .NET 5+ API contract
        // but cannot actually pre-allocate internal storage. The dictionary will resize on
        // demand as entries are added. This is a best-effort, API-compatible stub.
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2.ensurecapacity
        public int EnsureCapacity(int capacity) => Math.Max(capacity, dictionary.Count);
    }
}
#endif
