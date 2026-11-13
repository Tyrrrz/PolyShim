#if !FEATURE_MEMORY
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Buffers;

// https://learn.microsoft.com/dotnet/api/system.buffers.readonlysequence-1
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly partial struct ReadOnlySequence<T>(ReadOnlyMemory<T> memory, object positionToken)
{
    private readonly ReadOnlyMemory<T> _memory = memory;
    private readonly object _positionToken = positionToken;

    public ReadOnlySequence(T[]? array)
        : this(new ReadOnlyMemory<T>(array), array ?? EmptyToken) { }

    public ReadOnlySequence(T[]? array, int start, int length)
        : this(new ReadOnlyMemory<T>(array, start, length), array ?? EmptyToken) { }

    public ReadOnlySequence(ReadOnlyMemory<T> memory)
        : this(memory, memory.IsEmpty ? EmptyToken : new object()) { }

    internal ReadOnlySpan<T> Span => _memory.Span;

    public long Length => _memory.Length;

    public bool IsEmpty => _memory.IsEmpty;

    public SequencePosition Start => new(_positionToken, 0);

    public SequencePosition End => new(_positionToken, _memory.Length);

    public ReadOnlySequence<T> Slice(long start, long length)
    {
        if (start < 0 || start > _memory.Length)
            throw new ArgumentOutOfRangeException(nameof(start));
        if (length < 0 || length > _memory.Length - start)
            throw new ArgumentOutOfRangeException(nameof(length));

        return new ReadOnlySequence<T>(_memory.Slice((int)start, (int)length), _positionToken);
    }

    public ReadOnlySequence<T> Slice(long start) => Slice(start, _memory.Length - start);

    public ReadOnlySequence<T> Slice(SequencePosition start)
    {
        var startIndex = GetIndex(start);
        return Slice(startIndex);
    }

    public SequencePosition GetPosition(long offset, SequencePosition origin)
    {
        if (offset < 0)
            throw new ArgumentOutOfRangeException(nameof(offset));

        var originIndex = GetIndex(origin);
        var position = originIndex + offset;

        if (position < 0 || position > _memory.Length)
            throw new ArgumentOutOfRangeException(nameof(offset));

        return new SequencePosition(_positionToken, (int)position);
    }

    private int GetIndex(SequencePosition position)
    {
        if (!ReferenceEquals(position.GetObject(), _positionToken))
            throw new ArgumentOutOfRangeException(nameof(position));

        var index = position.GetInteger();
        if (index < 0 || index > _memory.Length)
            throw new ArgumentOutOfRangeException(nameof(position));

        return index;
    }
}

internal partial struct ReadOnlySequence<T>
{
    public static ReadOnlySequence<T> Empty => default;

    private static readonly object EmptyToken = new();
}
#endif
