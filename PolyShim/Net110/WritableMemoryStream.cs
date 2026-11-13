#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.IO;

// https://learn.microsoft.com/dotnet/api/system.io.writablememorystream
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class WritableMemoryStream(Memory<byte> buffer) : Stream
{
    private Memory<byte> _memory = buffer;
    private int _position;
    private int _length = buffer.Length;
    private bool _isDisposed;

    public override bool CanRead => !_isDisposed;

    public override bool CanSeek => !_isDisposed;

    public override bool CanWrite => !_isDisposed;

    public override long Length
    {
        get
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            return _length;
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

    public override int ReadByte()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        if (_position >= _length)
            return -1;

        return _memory.Span[_position++];
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

        var remaining = _length - _position;
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
            SeekOrigin.End => _length,
            _ => throw new ArgumentOutOfRangeException(nameof(origin)),
        };

        if (offset > int.MaxValue - basePosition)
            throw new ArgumentOutOfRangeException(nameof(offset));
        var target = basePosition + offset;
        if (target < 0)
            throw new IOException("Attempted to seek before the beginning of the stream.");

        _position = (int)target;

        return _position;
    }

    public override void SetLength(long value)
    {
        if (value < 0 || value > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value));

        ObjectDisposedException.ThrowIf(_isDisposed, this);

        var newLength = (int)value;
        if (newLength > _memory.Length)
            throw new NotSupportedException("Stream has fixed capacity.");

        if (newLength > _length)
            _memory.Span[_length..newLength].Clear();

        _length = newLength;
        if (_position > _length)
            _position = _length;
    }

    private void EnsureCapacity(int count)
    {
        if (count != 0 && _position > _memory.Length - count)
            throw new NotSupportedException("Stream has fixed capacity.");
    }

    public override void WriteByte(byte value)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
        EnsureCapacity(1);

        if (_position > _length)
            _memory.Span[_length.._position].Clear();

        _memory.Span[_position++] = value;

        if (_position > _length)
            _length = _position;
    }

    public override void Write(byte[] buffer, int offset, int count)
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

        if (count == 0)
            return;

        EnsureCapacity(count);

        if (_position > _length)
            _memory.Span[_length.._position].Clear();

        buffer.AsSpan(offset, count).CopyTo(_memory.Span[_position..]);
        _position += count;

        if (_position > _length)
            _length = _position;
    }

    public override void Flush() { }

    protected override void Dispose(bool disposing)
    {
        _isDisposed = true;
        _memory = default;
        base.Dispose(disposing);
    }
}
#endif
