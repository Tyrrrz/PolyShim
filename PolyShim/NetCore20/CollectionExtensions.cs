#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
using System.Diagnostics.CodeAnalysis;

// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Collections.Generic;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_CollectionExtensions
{
    // This weird conditional compilation pattern is needed so that List-Signatures.ps1 can pick up
    // both of the type receivers correctly.
#if !NETFRAMEWORK || NET45_OR_GREATER
    extension<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> dictionary)
    {
#else
    extension<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
    {
#endif
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.getvalueordefault#system-collections-generic-collectionextensions-getvalueordefault-2(system-collections-generic-ireadonlydictionary((-0-1))-0-1)
        public TValue? GetValueOrDefault(TKey key, TValue? defaultValue) =>
            dictionary.TryGetValue(key, out var value) ? value : defaultValue;

        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.getvalueordefault#system-collections-generic-collectionextensions-getvalueordefault-2(system-collections-generic-ireadonlydictionary((-0-1))-0)
        public TValue? GetValueOrDefault(TKey key) => dictionary.GetValueOrDefault(key, default);
    }
}
#endif
