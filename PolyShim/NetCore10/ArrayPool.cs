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
    public T[] Rent(int minimumLength) =>
        minimumLength >= 0
            ? new T[minimumLength]
            : throw new ArgumentOutOfRangeException(nameof(minimumLength));

    public void Return(T[] array, bool clearArray = false)
    {
        if (clearArray)
            Array.Clear(array, 0, array.Length);
    }

    public static ArrayPool<T> Shared { get; } = new();
}
#endif
