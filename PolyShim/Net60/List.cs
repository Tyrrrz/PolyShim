#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Collections.Generic;

internal static partial class PolyfillExtensions
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
