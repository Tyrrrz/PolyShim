#if (NETCOREAPP && !NET10_0_OR_GREATER) || (NETSTANDARD) || (NETFRAMEWORK && NET45_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net100_CollectionExtensions
{
#if NET7_0_OR_GREATER
    // These extensions are re-declared here as new-style extension members to ensure they take
    // priority over classic extension methods from the BCL when both are in scope. Without this,
    // C# 14's extension member resolution incorrectly resolves IList<T>.AsReadOnly() and
    // IDictionary<TKey,TValue>.AsReadOnly() to the ISet<T> overload below.
    extension<T>(IList<T> list)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.asreadonly#system-collections-generic-collectionextensions-asreadonly-1(system-collections-generic-ilist((-0)))
        public ReadOnlyCollection<T> AsReadOnly() => new ReadOnlyCollection<T>(list);
    }

    extension<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.asreadonly#system-collections-generic-collectionextensions-asreadonly-2(system-collections-generic-idictionary((-0-1)))
        public ReadOnlyDictionary<TKey, TValue> AsReadOnly() =>
            new ReadOnlyDictionary<TKey, TValue>(dictionary);
    }
#endif

    extension<T>(ISet<T> set)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.asreadonly#system-collections-generic-collectionextensions-asreadonly-1(system-collections-generic-iset((-0)))
        public ReadOnlySet<T> AsReadOnly() => new ReadOnlySet<T>(set);
    }
}
#endif
