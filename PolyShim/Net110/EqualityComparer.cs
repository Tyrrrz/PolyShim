#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net110_EqualityComparer
{
    extension<T>(EqualityComparer<T>)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.equalitycomparer-1.create?system-collections-generic-equalitycomparer-1-create-1(system-func((-0-0))-system-collections-generic-iequalitycomparer((-0)))
        public static EqualityComparer<T> Create<TKey>(
            Func<T?, TKey?> keySelector,
            IEqualityComparer<TKey>? keyComparer = null
        )
        {
            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));

            keyComparer ??= EqualityComparer<TKey>.Default;

            return EqualityComparer<T>.Create(
                (itemX, itemY) => keyComparer.Equals(keySelector(itemX)!, keySelector(itemY)!),
                item =>
                {
                    var key = keySelector(item);
                    return key is null ? 0 : keyComparer.GetHashCode(key);
                }
            );
        }
    }
}
#endif
