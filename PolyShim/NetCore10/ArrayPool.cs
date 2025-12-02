#if !FEATURE_ARRAYPOOL
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Buffers;

// https://learn.microsoft.com/dotnet/api/system.buffers.arraypool-1
[ExcludeFromCodeCoverage]
internal class ArrayPool<T>
{
    public static ArrayPool<T> Shared { get; } = new();

    public T[] Rent(int minimumLength)
    {
        if (minimumLength < 0)
            throw new ArgumentOutOfRangeException(nameof(minimumLength));

        return new T[minimumLength];
    }

    public void Return(T[] array, bool clearArray = false)
    {
        if (array is null)
            throw new ArgumentNullException(nameof(array));

        if (clearArray)
            Array.Clear(array, 0, array.Length);
    }
}
#endif
