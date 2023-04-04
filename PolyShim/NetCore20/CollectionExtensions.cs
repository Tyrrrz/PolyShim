#if (NETCOREAPP1_0_OR_GREATER && !NETCOREAPP2_0_OR_GREATER) || (NET20_OR_GREATER) || (NETSTANDARD1_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

[ExcludeFromCodeCoverage]
internal static class _9C779971A35845D5AE81C3FB2D09DEBC
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.collectionextensions.getvalueordefault#system-collections-generic-collectionextensions-getvalueordefault-2(system-collections-generic-ireadonlydictionary((-0-1))-0-1)
    public static TValue? GetValueOrDefault<TKey, TValue>(
#if NET20_OR_GREATER && !NET45_OR_GREATER
        this IDictionary<TKey, TValue> dictionary,
#else
        this IReadOnlyDictionary<TKey, TValue> dictionary,
#endif
        TKey key,
        TValue? defaultValue) =>
        dictionary.TryGetValue(key, out var value) ? value : defaultValue;

    // https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.collectionextensions.getvalueordefault#system-collections-generic-collectionextensions-getvalueordefault-2(system-collections-generic-ireadonlydictionary((-0-1))-0)
    public static TValue? GetValueOrDefault<TKey, TValue>(
#if NET20_OR_GREATER && !NET45_OR_GREATER
        this IDictionary<TKey, TValue> dictionary,
#else
        this IReadOnlyDictionary<TKey, TValue> dictionary,
#endif
        TKey key) =>
        dictionary.GetValueOrDefault(key, default);
}
#endif