#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

namespace System;

// These polyfills are provided separately because System.Memory package (which provides Span<T> and
// ReadOnlySpan<T> for older frameworks) does not include the Contains(T) method. This method was added
// to the BCL in .NET Core 2.1 / .NET Standard 2.1, but the System.Memory NuGet package doesn't backport it.
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_MemoryExtensions_Contains
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
