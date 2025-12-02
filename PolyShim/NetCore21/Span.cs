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

// https://learn.microsoft.com/dotnet/api/system.span-1
[ExcludeFromCodeCoverage]
internal readonly ref struct Span<T>
{
    private readonly T[] _array;
    private readonly int _offset;
    private readonly int _length;

    public Span(T[] array, int start, int length)
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

    public Span(T[] array)
        : this(array, 0, array.Length) { }

    public ref T this[int index]
    {
        get
        {
            if ((uint)index >= (uint)_length)
                throw new IndexOutOfRangeException();

            return ref _array[_offset + index];
        }
    }

    public int Length => _length;

    public bool IsEmpty => _length == 0;

    public static Span<T> Empty => default;

    public void Clear()
    {
        if (_array is not null)
            Array.Clear(_array, _offset, _length);
    }

    public void Fill(T value)
    {
        if (_array is not null)
        {
            for (var i = 0; i < _length; i++)
                _array[_offset + i] = value;
        }
    }

    public void CopyTo(Span<T> destination)
    {
        if (_length > destination._length)
            throw new ArgumentException("Destination is too short.", nameof(destination));

        if (_array is not null && destination._array is not null)
            Array.Copy(_array, _offset, destination._array, destination._offset, _length);
    }

    public bool TryCopyTo(Span<T> destination)
    {
        if (_length > destination._length)
            return false;

        if (_array is not null && destination._array is not null)
            Array.Copy(_array, _offset, destination._array, destination._offset, _length);

        return true;
    }

    public Span<T> Slice(int start)
    {
        if ((uint)start > (uint)_length)
            throw new ArgumentOutOfRangeException(nameof(start));

        return new Span<T>(_array, _offset + start, _length - start);
    }

    public Span<T> Slice(int start, int length)
    {
        if ((uint)start > (uint)_length || (uint)length > (uint)(_length - start))
            throw new ArgumentOutOfRangeException();

        return new Span<T>(_array, _offset + start, length);
    }

    public T[] ToArray()
    {
        if (_length == 0)
            return [];

        var result = new T[_length];
        if (_array is not null)
            Array.Copy(_array, _offset, result, 0, _length);

        return result;
    }

    public static implicit operator Span<T>(T[] array) => new(array);

    public static implicit operator Span<T>(ArraySegment<T> segment) =>
        new(segment.Array!, segment.Offset, segment.Count);

    public static implicit operator ReadOnlySpan<T>(Span<T> span) =>
        new(span._array, span._offset, span._length);

    public Enumerator GetEnumerator() => new(this);

    public ref struct Enumerator
    {
        private readonly Span<T> _span;
        private int _index;

        internal Enumerator(Span<T> span)
        {
            _span = span;
            _index = -1;
        }

        public bool MoveNext()
        {
            var index = _index + 1;
            if (index < _span.Length)
            {
                _index = index;
                return true;
            }

            return false;
        }

        public ref T Current => ref _span[_index];
    }
}
#endif
