#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_CollectionExtensions
{
    extension<TKey, TValue>(
#if !NETFRAMEWORK || NET45_OR_GREATER
        IReadOnlyDictionary<TKey, TValue> dictionary
#else
        IDictionary<TKey, TValue> dictionary
#endif
    )
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.getvalueordefault#system-collections-generic-collectionextensions-getvalueordefault-2(system-collections-generic-ireadonlydictionary((-0-1))-0-1)
        public TValue? GetValueOrDefault(TKey key, TValue? defaultValue) =>
            dictionary.TryGetValue(key, out var value) ? value : defaultValue;

        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.getvalueordefault#system-collections-generic-collectionextensions-getvalueordefault-2(system-collections-generic-ireadonlydictionary((-0-1))-0)
        public TValue? GetValueOrDefault(TKey key) => dictionary.GetValueOrDefault(key, default);
    }

    extension<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.tryadd
        public bool TryAdd(TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                return false;

            dictionary.Add(key, value);
            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.remove
        public bool Remove(TKey key, out TValue value)
        {
            if (dictionary.TryGetValue(key, out value!))
            {
                dictionary.Remove(key);
                return true;
            }

            value = default!;
            return false;
        }
    }
}
#endif
