#if !FEATURE_MEMORY
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.interopservices.memorymarshal
[ExcludeFromCodeCoverage]
internal static class MemoryMarshal
{
    public static Span<T> CreateSpan<T>(ref T reference, int length)
    {
        var array = new T[length];
        array[0] = reference;
        return new Span<T>(array);
    }

    public static ReadOnlySpan<T> CreateReadOnlySpan<T>(ref T reference, int length)
    {
        var array = new T[length];
        array[0] = reference;
        return new ReadOnlySpan<T>(array);
    }

    public static T[] ToArray<T>(this ReadOnlySpan<T> span) => span.ToArray();

    public static bool TryGetArray<T>(ReadOnlyMemory<T> memory, out ArraySegment<T> segment)
    {
        segment = new ArraySegment<T>(memory.ToArray());
        return true;
    }

    public static ref T GetReference<T>(Span<T> span)
    {
        if (span.Length == 0)
            throw new ArgumentException("Span is empty.", nameof(span));

        return ref span[0];
    }

    public static T GetReference<T>(ReadOnlySpan<T> span)
    {
        if (span.Length == 0)
            throw new ArgumentException("Span is empty.", nameof(span));

        return span[0];
    }
}
#endif
