#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq;

#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore30_EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.linq.enumerable.zip#system-linq-enumerable-zip-2(system-collections-generic-ienumerable((-0))-system-collections-generic-ienumerable((-1)))
        public IEnumerable<(T left, TOther right)> Zip<TOther>(IEnumerable<TOther> second) =>
            source.Zip(second, (x, y) => (x, y));
    }
}
#endif
