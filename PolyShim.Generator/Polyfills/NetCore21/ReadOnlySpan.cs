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

// https://learn.microsoft.com/dotnet/api/system.readonlyspan-1
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly ref struct ReadOnlySpan<T>
{
    private readonly T[]? _array;
    private readonly int _offset;

    public ReadOnlySpan(T[]? array, int start, int length)
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

    public ReadOnlySpan(T[]? array)
        : this(array, 0, array?.Length ?? 0) { }

    public T this[int index]
    {
        get
        {
            if (_array is null || index < 0 || index >= Length)
                throw new IndexOutOfRangeException();

            return _array[_offset + index];
        }
    }

    public int Length { get; }

    public bool IsEmpty => Length == 0;

    private Span<T> Span => new(_array, _offset, Length);

    public ReadOnlySpan<T> Slice(int start, int length)
    {
        if (start > Length || length > Length - start)
            throw new ArgumentOutOfRangeException();

        return new ReadOnlySpan<T>(_array, _offset + start, length);
    }

    public ReadOnlySpan<T> Slice(int start) => Slice(start, Length - start);

    public void CopyTo(Span<T> destination) => Span.CopyTo(destination);

    public bool TryCopyTo(Span<T> destination) => Span.TryCopyTo(destination);

    public T[] ToArray() => Span.ToArray();

    public static ReadOnlySpan<T> Empty => default;

    public static implicit operator ReadOnlySpan<T>(T[] array) => new(array);

    public static implicit operator ReadOnlySpan<T>(ArraySegment<T> segment) =>
        new(segment.Array, segment.Offset, segment.Count);

    public Enumerator GetEnumerator() => new(this);

    public ref struct Enumerator
    {
        private readonly ReadOnlySpan<T> _span;
        private int _index;

        internal Enumerator(ReadOnlySpan<T> span)
        {
            _span = span;
            _index = -1;
        }

        public T Current => _span[_index];

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
