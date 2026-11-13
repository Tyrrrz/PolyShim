#if !FEATURE_MEMORY
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Buffers;

// https://learn.microsoft.com/dotnet/api/system.buffers.readonlysequence-1
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly partial struct ReadOnlySequence<T>(
    ReadOnlyMemory<T> memory,
    object positionToken,
    int startIndex
)
{
    private readonly ReadOnlyMemory<T> _memory = memory;
    private readonly object _positionToken = positionToken;
    private readonly int _startIndex = startIndex;

    public ReadOnlySequence(T[] array)
        : this(
            new ReadOnlyMemory<T>(array ?? throw new ArgumentNullException(nameof(array))),
            array,
            0
        ) { }

    public ReadOnlySequence(T[] array, int start, int length)
        : this(
            new ReadOnlyMemory<T>(
                array ?? throw new ArgumentNullException(nameof(array)),
                start,
                length
            ),
            array,
            start
        ) { }

    public ReadOnlySequence(ReadOnlyMemory<T> memory)
        : this(memory, memory.IsEmpty ? EmptyToken : new object(), 0) { }

    public ReadOnlySequence(
        ReadOnlySequenceSegment<T> startSegment,
        int startIndex,
        ReadOnlySequenceSegment<T> endSegment,
        int endIndex
    )
        : this(
            BuildMemory(
                startSegment ?? throw new ArgumentNullException(nameof(startSegment)),
                startIndex,
                endSegment ?? throw new ArgumentNullException(nameof(endSegment)),
                endIndex
            ),
            new object(),
            0
        ) { }

    internal ReadOnlySpan<T> Span => _memory.Span;

    public long Length => _memory.Length;

    public bool IsEmpty => _memory.IsEmpty;

    public SequencePosition Start => new(_positionToken, _startIndex);

    public SequencePosition End => new(_positionToken, _startIndex + _memory.Length);

    public ReadOnlySequence<T> Slice(long start, long length)
    {
        if (start < 0 || start > _memory.Length)
            throw new ArgumentOutOfRangeException(nameof(start));
        if (length < 0 || length > _memory.Length - start)
            throw new ArgumentOutOfRangeException(nameof(length));

        return new ReadOnlySequence<T>(
            _memory.Slice((int)start, (int)length),
            _positionToken,
            _startIndex + (int)start
        );
    }

    public ReadOnlySequence<T> Slice(long start) => Slice(start, _memory.Length - start);

    public ReadOnlySequence<T> Slice(SequencePosition start)
    {
        var startIndex = GetRelativeIndex(start);
        return Slice(startIndex);
    }

    public SequencePosition GetPosition(long offset, SequencePosition origin)
    {
        if (offset < 0)
            throw new ArgumentOutOfRangeException(nameof(offset));

        var originIndex = GetAbsoluteIndex(origin);
        var position = originIndex + offset;

        if (position < _startIndex || position > _startIndex + (long)_memory.Length)
            throw new ArgumentOutOfRangeException(nameof(offset));

        return new SequencePosition(_positionToken, (int)position);
    }

    private int GetRelativeIndex(SequencePosition position)
    {
        var absoluteIndex = GetAbsoluteIndex(position);
        return absoluteIndex - _startIndex;
    }

    private int GetAbsoluteIndex(SequencePosition position)
    {
        if (!ReferenceEquals(position.GetObject(), _positionToken))
            throw new ArgumentOutOfRangeException(nameof(position));

        var index = position.GetInteger();
        if (index < _startIndex || index > _startIndex + _memory.Length)
            throw new ArgumentOutOfRangeException(nameof(position));

        return index;
    }

    private static ReadOnlyMemory<T> BuildMemory(
        ReadOnlySequenceSegment<T> startSegment,
        int startIndex,
        ReadOnlySequenceSegment<T> endSegment,
        int endIndex
    )
    {
        if (startIndex < 0 || startIndex > startSegment.Memory.Length)
            throw new ArgumentOutOfRangeException(nameof(startIndex));
        if (endIndex < 0 || endIndex > endSegment.Memory.Length)
            throw new ArgumentOutOfRangeException(nameof(endIndex));

        var current = startSegment;
        var totalLength = 0;

        while (true)
        {
            if (ReferenceEquals(current, endSegment))
            {
                if (ReferenceEquals(startSegment, endSegment) && endIndex < startIndex)
                    throw new ArgumentOutOfRangeException(nameof(endIndex));

                totalLength +=
                    endIndex - (ReferenceEquals(startSegment, endSegment) ? startIndex : 0);
                break;
            }

            totalLength +=
                current.Memory.Length - (ReferenceEquals(current, startSegment) ? startIndex : 0);

            current = current.Next ?? throw new ArgumentOutOfRangeException(nameof(endSegment));
        }

        var array = new T[totalLength];
        var written = 0;
        current = startSegment;

        while (true)
        {
            if (ReferenceEquals(current, endSegment))
            {
                var source = current.Memory.Span.Slice(
                    ReferenceEquals(startSegment, endSegment) ? startIndex : 0,
                    endIndex - (ReferenceEquals(startSegment, endSegment) ? startIndex : 0)
                );
                source.CopyTo(array.AsSpan(written));
                break;
            }

            var segmentStart = ReferenceEquals(current, startSegment) ? startIndex : 0;
            var sourceSpan = current.Memory.Span.Slice(segmentStart);
            sourceSpan.CopyTo(array.AsSpan(written));
            written += sourceSpan.Length;

            current = current.Next!;
        }

        return new ReadOnlyMemory<T>(array);
    }
}

internal partial struct ReadOnlySequence<T>
{
    public static ReadOnlySequence<T> Empty => default;

    private static readonly object EmptyToken = new();
}
#endif
