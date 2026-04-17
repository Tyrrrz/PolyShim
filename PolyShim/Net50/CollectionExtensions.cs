#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_CollectionExtensions
{
    extension<T>(IList<T> list)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.asreadonly#system-collections-generic-collectionextensions-asreadonly-1(system-collections-generic-ilist((-0)))
        public ReadOnlyCollection<T> AsReadOnly() => new ReadOnlyCollection<T>(list);
    }

#if !NETFRAMEWORK || NET45_OR_GREATER
    extension<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.asreadonly#system-collections-generic-collectionextensions-asreadonly-2(system-collections-generic-idictionary((-0-1)))
        public ReadOnlyDictionary<TKey, TValue> AsReadOnly() =>
            new ReadOnlyDictionary<TKey, TValue>(dictionary);
    }
#endif
}
#endif
