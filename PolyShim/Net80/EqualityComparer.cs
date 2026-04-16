#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net80_EqualityComparer
{
    private class DelegateEqualityComparer<T>(Func<T?, T?, bool> equals, Func<T, int>? getHashCode)
        : EqualityComparer<T>
    {
        public override bool Equals(T? x, T? y) => equals(x, y);

        public override int GetHashCode(T obj) =>
            getHashCode is not null ? getHashCode(obj) : throw new NotSupportedException();
    }

    extension<T>(EqualityComparer<T>)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.equalitycomparer-1.create
        public static EqualityComparer<T> Create(
            Func<T?, T?, bool> equals,
            Func<T, int>? getHashCode = null
        ) => new DelegateEqualityComparer<T>(equals, getHashCode);
    }
}
#endif
