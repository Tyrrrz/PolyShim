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
    extension<T>(ISet<T> set)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.collectionextensions.asreadonly#system-collections-generic-collectionextensions-asreadonly-1(system-collections-generic-iset((-0)))
        public ReadOnlySet<T> AsReadOnly() => new ReadOnlySet<T>(set);
    }
}
#endif
