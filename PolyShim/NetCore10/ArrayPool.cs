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
