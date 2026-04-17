#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETSTANDARD) || (NETFRAMEWORK && NET45_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.ObjectModel;

// https://learn.microsoft.com/dotnet/api/system.collections.objectmodel.readonlyset-1
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class ReadOnlySet<T> : IReadOnlySet<T>
{
    private readonly ISet<T> _set;

    public ReadOnlySet(ISet<T> set) => _set = set;

    public int Count => _set.Count;

    public bool Contains(T item) => _set.Contains(item);

    public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_set).GetEnumerator();

    public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);

    public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);

    public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);

    public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);

    public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);

    public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);
}
#endif
