#if FEATURE_MEMORY && (NETFRAMEWORK || (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER))
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;

namespace System;

internal static partial class PolyfillExtensions
{
    extension<T>(Span<T> span)
    {
        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.contains#system-memoryextensions-contains-1(system-span((-0))-0)
        public bool Contains(T value)
        {
            var comparer = EqualityComparer<T>.Default;
            foreach (var item in span)
            {
                if (comparer.Equals(item, value))
                    return true;
            }

            return false;
        }
    }

    extension<T>(ReadOnlySpan<T> span)
    {
        // https://learn.microsoft.com/dotnet/api/system.memoryextensions.contains#system-memoryextensions-contains-1(system-readonlyspan((-0))-0)
        public bool Contains(T value)
        {
            var comparer = EqualityComparer<T>.Default;
            foreach (var item in span)
            {
                if (comparer.Equals(item, value))
                    return true;
            }

            return false;
        }
    }
}
#endif
