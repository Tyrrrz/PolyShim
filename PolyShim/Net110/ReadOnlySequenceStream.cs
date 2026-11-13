#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Buffers;

// https://learn.microsoft.com/dotnet/api/system.buffers.readonlysequencestream
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class ReadOnlySequenceStream(ReadOnlySequence<byte> source) : Stream
{
    private ReadOnlySequence<byte> _sequence = source;
    private SequencePosition _position = source.Start;
    private bool _isDisposed;

    public override bool CanRead => !_isDisposed;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length =>
        throw new NotSupportedException("Stream does not support seeking.");

    public override long Position
    {
        get => throw new NotSupportedException("Stream does not support seeking.");
        set => throw new NotSupportedException("Stream does not support seeking.");
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (buffer is null)
            throw new ArgumentNullException(nameof(buffer));
        if (offset < 0)
            throw new ArgumentOutOfRangeException(nameof(offset));
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));
        if (buffer.Length - offset < count)
            throw new ArgumentException("Offset and length were out of bounds for the array.");

        ObjectDisposedException.ThrowIf(_isDisposed, this);

        var remaining = _sequence.Slice(_position);
        var bytesToRead = (int)Math.Min(remaining.Length, count);
        if (bytesToRead <= 0)
            return 0;

        remaining.Slice(0, bytesToRead).CopyTo(buffer.AsSpan(offset, bytesToRead));
        _position = _sequence.GetPosition(bytesToRead, _position);
        return bytesToRead;
    }

    public override int ReadByte()
    {
        var buffer = new byte[1];
        return Read(buffer, 0, 1) == 1 ? buffer[0] : -1;
    }

    public override long Seek(long offset, SeekOrigin origin) =>
        throw new NotSupportedException("Stream does not support seeking.");

    public override void SetLength(long value) =>
        throw new NotSupportedException("Stream does not support writing.");

    public override void Write(byte[] buffer, int offset, int count) =>
        throw new NotSupportedException("Stream does not support writing.");

    public override void Flush() { }

    protected override void Dispose(bool disposing)
    {
        _isDisposed = true;
        _sequence = default;
        base.Dispose(disposing);
    }
}
#endif
