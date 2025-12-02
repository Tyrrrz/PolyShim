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

// https://learn.microsoft.com/dotnet/api/system.memory-1
[ExcludeFromCodeCoverage]
internal readonly struct Memory<T> : IEquatable<Memory<T>>
{
    private readonly T[] _array;
    private readonly int _offset;
    private readonly int _length;

    public Memory(T[] array, int start, int length)
    {
        if (array is null)
            throw new ArgumentNullException(nameof(array));
        if (start < 0 || start > array.Length)
            throw new ArgumentOutOfRangeException(nameof(start));
        if (length < 0 || start + length > array.Length)
            throw new ArgumentOutOfRangeException(nameof(length));

        _array = array;
        _offset = start;
        _length = length;
    }

    public Memory(T[] array)
        : this(array, 0, array.Length) { }

    public int Length => _length;

    public bool IsEmpty => _length == 0;

    public static Memory<T> Empty => default;

    public Span<T> Span => new(_array, _offset, _length);

    public Memory<T> Slice(int start)
    {
        if ((uint)start > (uint)_length)
            throw new ArgumentOutOfRangeException(nameof(start));

        return new Memory<T>(_array, _offset + start, _length - start);
    }

    public Memory<T> Slice(int start, int length)
    {
        if ((uint)start > (uint)_length || (uint)length > (uint)(_length - start))
            throw new ArgumentOutOfRangeException();

        return new Memory<T>(_array, _offset + start, length);
    }

    public void CopyTo(Memory<T> destination) => Span.CopyTo(destination.Span);

    public bool TryCopyTo(Memory<T> destination) => Span.TryCopyTo(destination.Span);

    public T[] ToArray() => Span.ToArray();

    public override bool Equals(object? obj) => obj is Memory<T> memory && Equals(memory);

    public bool Equals(Memory<T> other) =>
        _array == other._array && _offset == other._offset && _length == other._length;

    public override int GetHashCode()
    {
        var hash = 17;
        if (_array is not null)
            hash = hash * 31 + _array.GetHashCode();
        hash = hash * 31 + _offset.GetHashCode();
        hash = hash * 31 + _length.GetHashCode();
        return hash;
    }

    public static implicit operator Memory<T>(T[] array) => new(array);

    public static implicit operator Memory<T>(ArraySegment<T> segment) =>
        new(segment.Array!, segment.Offset, segment.Count);

    public static implicit operator ReadOnlyMemory<T>(Memory<T> memory) =>
        new(memory._array, memory._offset, memory._length);
}
#endif
