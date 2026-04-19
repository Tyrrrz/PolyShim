#if NETFRAMEWORK && !NET45_OR_GREATER
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.weakreference-1
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class WeakReference<T>(T target, bool trackResurrection)
    where T : class
{
    private readonly WeakReference _reference = new(target, trackResurrection)

    public WeakReference(T target)
        : this(target, false) { }

    public void SetTarget(T target) => _reference.Target = target;

    public bool TryGetTarget([NotNullWhen(true)] out T? target)
    {
        target = _reference.Target as T;
        return target is not null;
    }
}
#endif
