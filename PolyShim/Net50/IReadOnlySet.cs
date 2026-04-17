#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETSTANDARD) || (NETFRAMEWORK)
#nullable enable
#pragma warning disable CS0436

namespace System.Collections.Generic;

#if !NETFRAMEWORK || NET45_OR_GREATER
// IReadOnlyCollection<T> (base interface) is not available in .NET Framework below 4.5
// https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlyset-1
internal interface IReadOnlySet<T> : IReadOnlyCollection<T>
{
    bool Contains(T item);

    bool IsProperSubsetOf(IEnumerable<T> other);

    bool IsProperSupersetOf(IEnumerable<T> other);

    bool IsSubsetOf(IEnumerable<T> other);

    bool IsSupersetOf(IEnumerable<T> other);

    bool Overlaps(IEnumerable<T> other);

    bool SetEquals(IEnumerable<T> other);
}
#endif
#endif
