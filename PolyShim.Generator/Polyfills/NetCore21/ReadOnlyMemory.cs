#if !FEATURE_MEMORY
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.readonlymemory-1
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct ReadOnlyMemory<T> : IEquatable<ReadOnlyMemory<T>>
{
    private readonly T[]? _array;
    private readonly int _offset;

    public ReadOnlyMemory(T[]? array, int start, int length)
    {
        if (array is null)
            return;

        if (start < 0 || start > array.Length)
            throw new ArgumentOutOfRangeException(nameof(start));
        if (length < 0 || start + length > array.Length)
            throw new ArgumentOutOfRangeException(nameof(length));

        _array = array;
        _offset = start;
        Length = length;
    }

    public ReadOnlyMemory(T[]? array)
        : this(array, 0, array?.Length ?? 0) { }

    public int Length { get; }

    public bool IsEmpty => Length == 0;

    public ReadOnlySpan<T> Span => new(_array, _offset, Length);

    public ReadOnlyMemory<T> Slice(int start, int length)
    {
        if (start > Length || length > Length - start)
            throw new ArgumentOutOfRangeException();

        return new ReadOnlyMemory<T>(_array, _offset + start, length);
    }

    public ReadOnlyMemory<T> Slice(int start) => Slice(start, Length - start);

    public void CopyTo(Memory<T> destination) => Span.CopyTo(destination.Span);

    public bool TryCopyTo(Memory<T> destination) => Span.TryCopyTo(destination.Span);

    public T[] ToArray() => Span.ToArray();

    public override bool Equals(object? obj) => obj is ReadOnlyMemory<T> memory && Equals(memory);

    public bool Equals(ReadOnlyMemory<T> other) =>
        _array == other._array && _offset == other._offset && Length == other.Length;

    public override int GetHashCode() => HashCode.Combine(_array, _offset, Length);

    public static ReadOnlyMemory<T> Empty => default;

    public static implicit operator ReadOnlyMemory<T>(T[] array) => new(array);

    public static implicit operator ReadOnlyMemory<T>(ArraySegment<T> segment) =>
        new(segment.Array, segment.Offset, segment.Count);
}
#endif
