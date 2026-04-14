#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net60_List
{
    extension<T>(List<T> list)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.list-1.ensurecapacity
        public int EnsureCapacity(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            if (list.Capacity < capacity)
                list.Capacity = capacity;

            return list.Capacity;
        }
    }
}
#endif
