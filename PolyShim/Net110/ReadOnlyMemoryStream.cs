#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.IO;

// https://learn.microsoft.com/dotnet/api/system.io.readonlymemorystream
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class ReadOnlyMemoryStream(ReadOnlyMemory<byte> source) : Stream
{
    private ReadOnlyMemory<byte> _memory = source;
    private int _position;
    private bool _isDisposed;

    public override bool CanRead => !_isDisposed;

    public override bool CanSeek => !_isDisposed;

    public override bool CanWrite => false;

    public override long Length
    {
        get
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            return _memory.Length;
        }
    }

    public override long Position
    {
        get
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            return _position;
        }
        set
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            if (value < 0 || value > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value));

            _position = (int)value;
        }
    }

    public override void Flush() { }

    public override int ReadByte()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        var span = _memory.Span;
        if (_position >= span.Length)
            return -1;

        return span[_position++];
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

        var remaining = _memory.Length - _position;
        if (remaining <= 0 || count == 0)
            return 0;

        var bytesToRead = Math.Min(remaining, count);
        _memory.Span.Slice(_position, bytesToRead).CopyTo(buffer.AsSpan(offset, bytesToRead));
        _position += bytesToRead;
        return bytesToRead;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        var basePosition = origin switch
        {
            SeekOrigin.Begin => 0L,
            SeekOrigin.Current => _position,
            SeekOrigin.End => _memory.Length,
            _ => throw new ArgumentOutOfRangeException(nameof(origin)),
        };

        var target = basePosition + offset;
        if (target > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(offset));
        if (target < 0)
            throw new IOException("Attempted to seek before the beginning of the stream.");

        _position = (int)target;
        return _position;
    }

    public override void SetLength(long value) =>
        throw new NotSupportedException("Stream does not support writing.");

    public override void Write(byte[] buffer, int offset, int count) =>
        throw new NotSupportedException("Stream does not support writing.");

    protected override void Dispose(bool disposing)
    {
        _isDisposed = true;
        _memory = default;
        base.Dispose(disposing);
    }
}
#endif
