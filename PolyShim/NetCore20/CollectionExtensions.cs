#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Collections.Generic;

internal static partial class PolyfillExtensions
{
    extension<TKey, TValue>(
#if NETFRAMEWORK && !NET45_OR_GREATER
        IDictionary<TKey, TValue> dictionary
#else
        IReadOnlyDictionary<TKey, TValue> dictionary
#endif
    )
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.getvalueordefault#system-collections-generic-collectionextensions-getvalueordefault-2(system-collections-generic-ireadonlydictionary((-0-1))-0-1)
        public TValue? GetValueOrDefault(TKey key, TValue? defaultValue) =>
            dictionary.TryGetValue(key, out var value) ? value : defaultValue;

        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.getvalueordefault#system-collections-generic-collectionextensions-getvalueordefault-2(system-collections-generic-ireadonlydictionary((-0-1))-0)
        public TValue? GetValueOrDefault(TKey key) => dictionary.GetValueOrDefault(key, default);
    }
}
#endif
