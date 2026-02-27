#if !FEATURE_MEMORY
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections;
using System.Collections.Generic;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif
using System.Runtime.CompilerServices;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.span-1
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly ref struct Span<T>
{
    private readonly T[]? _array;
    private readonly int _offset;

    public Span(T[]? array, int start, int length)
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

    public Span(T[]? array)
        : this(array, 0, array?.Length ?? 0) { }

#if ALLOW_UNSAFE_BLOCKS
    public unsafe Span(void* pointer, int length)
        : this(new T[length])
    {
        // Assume that we're given a block of memory for which this cast is valid
#pragma warning disable CS8500
        var source = (T*)pointer;
#pragma warning restore CS8500

        for (var i = 0; i < length; i++)
            _array![i] = source[i];
    }
#endif

    public ref T this[int index]
    {
        get
        {
            if (_array is null || index < 0 || index >= Length)
                throw new IndexOutOfRangeException();

            return ref _array[_offset + index];
        }
    }

    public int Length { get; }

    public bool IsEmpty => Length == 0;

    public Span<T> Slice(int start, int length)
    {
        if (start > Length || length > Length - start)
            throw new ArgumentOutOfRangeException();

        return new Span<T>(_array, _offset + start, length);
    }

    public Span<T> Slice(int start) => Slice(start, Length - start);

    public void Clear()
    {
        if (_array is null)
            return;

        Array.Clear(_array, _offset, Length);
    }

    public void Fill(T value)
    {
        if (_array is null)
            return;

        for (var i = 0; i < Length; i++)
            _array[_offset + i] = value;
    }

    public void CopyTo(Span<T> destination)
    {
        if (Length > destination.Length)
            throw new ArgumentException("Destination is too short.", nameof(destination));

        if (_array is not null && destination._array is not null)
            Array.Copy(_array, _offset, destination._array, destination._offset, Length);
    }

    public bool TryCopyTo(Span<T> destination)
    {
        if (Length > destination.Length)
            return false;

        if (_array is not null && destination._array is not null)
            Array.Copy(_array, _offset, destination._array, destination._offset, Length);

        return true;
    }

    public T[] ToArray()
    {
        if (Length == 0 || _array is null)
            return [];

        var result = new T[Length];
        Array.Copy(_array, _offset, result, 0, Length);

        return result;
    }

    public static Span<T> Empty => default;

    public static implicit operator Span<T>(T[] array) => new(array);

    public static implicit operator Span<T>(ArraySegment<T> segment) =>
        new(segment.Array, segment.Offset, segment.Count);

    public static implicit operator ReadOnlySpan<T>(Span<T> span) =>
        new(span._array, span._offset, span.Length);

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

        public ref T Current => ref _span[_index];

        public bool MoveNext()
        {
            var index = _index + 1;
            if (index >= _span.Length)
                return false;

            _index = index;
            return true;
        }
    }
}
#endif
