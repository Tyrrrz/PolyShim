#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System.IO;

// https://learn.microsoft.com/dotnet/api/system.io.stringstream
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class StringStream(ReadOnlyMemory<char> text, Encoding encoding) : Stream
{
    private readonly ReadOnlyMemory<char> _text = text;

    private readonly Encoding _encoding =
        encoding ?? throw new ArgumentNullException(nameof(encoding));

    private readonly Encoder _encoder = encoding.GetEncoder();
    private readonly byte[] _buffer = new byte[encoding.GetMaxByteCount(1)];
    private int _bufferOffset;
    private int _bufferCount;
    private int _charPosition;

    private bool _isDisposed;

    public StringStream(string text, Encoding encoding)
        : this(text?.AsMemory() ?? throw new ArgumentNullException(nameof(text)), encoding) { }

    public Encoding Encoding => _encoding;

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

        var totalBytesRead = 0;
        while (totalBytesRead < count)
        {
            // Drain bytes left over from the previously encoded character
            if (_bufferCount > 0)
            {
                var bytesToCopy = Math.Min(_bufferCount, count - totalBytesRead);

                Buffer.BlockCopy(
                    _buffer,
                    _bufferOffset,
                    buffer,
                    offset + totalBytesRead,
                    bytesToCopy
                );

                _bufferOffset += bytesToCopy;
                _bufferCount -= bytesToCopy;
                totalBytesRead += bytesToCopy;

                continue;
            }

            if (_charPosition >= _text.Length)
                break;

            // Encode one character at a time. The encoder tracks surrogate state across calls,
            // and the final character is flushed so that a trailing lone surrogate is reported.
            _encoder.Convert(
                _text.Span.Slice(_charPosition, 1),
                _buffer,
                _charPosition == _text.Length - 1,
                out _,
                out var bytesUsed,
                out _
            );

            _charPosition++;
            _bufferOffset = 0;
            _bufferCount = bytesUsed;
        }

        return totalBytesRead;
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
        base.Dispose(disposing);
    }
}
#endif
