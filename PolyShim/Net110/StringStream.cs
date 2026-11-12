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
    private const int ChunkSize = 1024;

    private readonly ReadOnlyMemory<char> _text = text;

    private readonly Encoding _encoding =
        encoding ?? throw new ArgumentNullException(nameof(encoding));

    private Encoder? _encoder;
    private char[]? _charBuffer;
    private byte[]? _pendingBytes;
    private int _pendingOffset;
    private int _pendingCount;
    private int _charPosition;
    private bool _encoderFlushed;
    private bool _disposed;

    public StringStream(string text, Encoding encoding)
        : this((text ?? throw new ArgumentNullException(nameof(text))).AsMemory(), encoding) { }

    public Encoding Encoding => _encoding;

    public override bool CanRead => !_disposed;

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

        ObjectDisposedException.ThrowIf(_disposed, this);

        var totalBytesRead = 0;

        while (totalBytesRead < count)
        {
            if (_pendingCount == 0)
            {
                if (_encoderFlushed)
                    break;

                // Encode the next chunk of characters into the pending buffer. Stateful encoders
                // (and lone surrogates at chunk boundaries) are handled by the Encoder, which is
                // flushed on the last chunk. A chunk may yield no bytes, so keep looping.
                _charBuffer ??= new char[ChunkSize];
                _pendingBytes ??= new byte[_encoding.GetMaxByteCount(ChunkSize)];

                var charCount = Math.Min(_text.Length - _charPosition, ChunkSize);

                _text
                    .Span.Slice(_charPosition, charCount)
                    .CopyTo(new Span<char>(_charBuffer, 0, charCount));

                _charPosition += charCount;
                var isFinalChunk = _charPosition >= _text.Length;

                _encoder ??= _encoding.GetEncoder();
                _pendingOffset = 0;
                _pendingCount = _encoder.GetBytes(
                    _charBuffer,
                    0,
                    charCount,
                    _pendingBytes,
                    0,
                    isFinalChunk
                );

                if (isFinalChunk)
                    _encoderFlushed = true;

                continue;
            }

            var bytesToCopy = Math.Min(_pendingCount, count - totalBytesRead);

            Buffer.BlockCopy(
                _pendingBytes!,
                _pendingOffset,
                buffer,
                offset + totalBytesRead,
                bytesToCopy
            );

            _pendingOffset += bytesToCopy;
            _pendingCount -= bytesToCopy;
            totalBytesRead += bytesToCopy;
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
        _disposed = true;
        base.Dispose(disposing);
    }
}
#endif
