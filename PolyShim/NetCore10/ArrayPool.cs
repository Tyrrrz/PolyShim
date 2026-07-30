#if !FEATURE_ARRAYPOOL
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Buffers;

// https://learn.microsoft.com/dotnet/api/system.buffers.arraypool-1
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class ArrayPool<T>
{
    // One array slot per thread per T — covers the dominant sequential rent/return pattern
    // with zero synchronization overhead.
    [ThreadStatic]
    private static T[]? _cached;

    public T[] Rent(int minimumLength)
    {
        if (minimumLength < 0)
            throw new ArgumentOutOfRangeException(nameof(minimumLength));

        var cached = _cached;
        if (cached is not null && cached.Length >= minimumLength)
        {
            _cached = null;
            return cached;
        }

        return new T[minimumLength];
    }

    public void Return(T[] array, bool clearArray = false)
    {
        if (clearArray)
            Array.Clear(array, 0, array.Length);

        _cached = array;
    }

    public static ArrayPool<T> Shared { get; } = new();
}
#endif
