#if (NETCOREAPP && !NET6_0_OR_GREATER) || NETFRAMEWORK || NETSTANDARD
// ZLibStream is also available on .NET Framework 4.5+ and .NET Standard 1.1+
#if (!NETFRAMEWORK || NET45_OR_GREATER) && (!NETSTANDARD || NETSTANDARD1_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.IO.Compression;

// https://learn.microsoft.com/dotnet/api/system.io.compression.zlibstream
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class ZLibStream : Stream
{
    private const byte HeaderCmf = 0x78;
    private const uint Adler32Mod = 65521;

    private readonly Stream _baseStream;
    private readonly bool _leaveOpen;
    private readonly DeflateStream _deflateStream;
    private readonly CompressionMode _mode;
    private readonly byte _headerFlg;
    private readonly TrailerHoldbackStream? _trailerStream;

    private bool _isHeaderProcessed;
    private bool _isEmptySource;
    private bool _isTrailerValidated;
    private bool _isDisposed;

    // Adler-32 checksum of the uncompressed data, maintained as data flows through the stream.
    private uint _adlerA = 1;
    private uint _adlerB;

    public ZLibStream(Stream stream, CompressionMode mode, bool leaveOpen)
    {
        _baseStream = stream;
        _leaveOpen = leaveOpen;
        _mode = mode;
        _headerFlg = GetHeaderFlg(CompressionLevel.Optimal);

        if (mode == CompressionMode.Decompress)
        {
            _trailerStream = new TrailerHoldbackStream(stream);
            _deflateStream = new DeflateStream(_trailerStream, mode, leaveOpen: true);
        }
        else
        {
            _deflateStream = new DeflateStream(stream, mode, leaveOpen: true);
        }
    }

    public ZLibStream(Stream stream, CompressionMode mode)
        : this(stream, mode, leaveOpen: false) { }

    public ZLibStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen)
    {
        _baseStream = stream;
        _leaveOpen = leaveOpen;
        _mode = CompressionMode.Compress;
        _headerFlg = GetHeaderFlg(compressionLevel);
        _deflateStream = new DeflateStream(stream, compressionLevel, leaveOpen: true);
    }

    public ZLibStream(Stream stream, CompressionLevel compressionLevel)
        : this(stream, compressionLevel, leaveOpen: false) { }

    public override bool CanRead => !_isDisposed && _deflateStream.CanRead;

    public override bool CanWrite => !_isDisposed && _deflateStream.CanWrite;

    public override bool CanSeek => false;

    public override long Length => throw new NotSupportedException();

    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override void Flush()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
        _deflateStream.Flush();
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override int Read(byte[] buffer, int offset, int count)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        if (_mode != CompressionMode.Decompress)
            return _deflateStream.Read(buffer, offset, count);

        if (count == 0)
            return 0;

        EnsureHeaderRead();
        if (_isEmptySource)
            return 0;

        var bytesRead = _deflateStream.Read(buffer, offset, count);

        if (bytesRead > 0)
            UpdateAdler32(buffer, offset, bytesRead);
        else
            ValidateTrailer();

        return bytesRead;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        if (_mode == CompressionMode.Compress)
        {
            if (count == 0)
                return;

            EnsureHeaderWritten();
            UpdateAdler32(buffer, offset, count);
        }

        _deflateStream.Write(buffer, offset, count);
    }

    protected override void Dispose(bool disposing)
    {
        try
        {
            if (!_isDisposed && disposing)
            {
                try
                {
                    _deflateStream.Dispose();

                    if (_mode == CompressionMode.Compress && _isHeaderProcessed)
                        WriteTrailer();
                }
                finally
                {
                    if (!_leaveOpen)
                        _baseStream.Dispose();
                }
            }
        }
        finally
        {
            _isDisposed = true;
            base.Dispose(disposing);
        }
    }

    private void EnsureHeaderWritten()
    {
        if (_isHeaderProcessed)
            return;

        _baseStream.WriteByte(HeaderCmf);
        _baseStream.WriteByte(_headerFlg);

        _isHeaderProcessed = true;
    }

    private void EnsureHeaderRead()
    {
        if (_isHeaderProcessed)
            return;

        var cmf = _baseStream.ReadByte();
        if (cmf < 0)
        {
            _isHeaderProcessed = true;
            _isEmptySource = true;
            return;
        }

        var flg = _baseStream.ReadByte();

        // Method must be deflate, window size must be valid, and preset dictionaries are unsupported
        if (
            flg < 0
            || (cmf & 0x0F) != 8
            || (cmf >> 4) > 7
            || (flg & 0x20) != 0
            || ((cmf << 8) | flg) % 31 != 0
        )
        {
            throw new InvalidDataException("The input stream is not a valid ZLib stream.");
        }

        _isHeaderProcessed = true;
    }

    private void ValidateTrailer()
    {
        if (_isTrailerValidated)
            return;

        _isTrailerValidated = true;

        var trailer = _trailerStream!.ReadTrailer();
        var checksum = (_adlerB << 16) | _adlerA;

        if (
            trailer is null
            || trailer[0] != (byte)(checksum >> 24)
            || trailer[1] != (byte)(checksum >> 16)
            || trailer[2] != (byte)(checksum >> 8)
            || trailer[3] != (byte)checksum
        )
        {
            throw new InvalidDataException("The ZLib stream checksum is missing or invalid.");
        }
    }

    private void WriteTrailer()
    {
        var checksum = (_adlerB << 16) | _adlerA;

        _baseStream.WriteByte((byte)(checksum >> 24));
        _baseStream.WriteByte((byte)(checksum >> 16));
        _baseStream.WriteByte((byte)(checksum >> 8));
        _baseStream.WriteByte((byte)checksum);
    }

    private void UpdateAdler32(byte[] buffer, int offset, int count)
    {
        var a = _adlerA;
        var b = _adlerB;

        for (var i = 0; i < count; i++)
        {
            a = (a + buffer[offset + i]) % Adler32Mod;
            b = (b + a) % Adler32Mod;
        }

        _adlerA = a;
        _adlerB = b;
    }

    private static byte GetHeaderFlg(CompressionLevel compressionLevel)
    {
        // Map the compression level to the zlib FLEVEL value (bits 6-7 of FLG).
        // Known levels match the values produced by the native zlib library;
        // any other (e.g. future) level defaults to the maximum compression flag.
        var flevel = (int)compressionLevel switch
        {
            2 => 0, // CompressionLevel.NoCompression
            1 => 0, // CompressionLevel.Fastest
            0 => 2, // CompressionLevel.Optimal
            _ => 3, // CompressionLevel.SmallestSize (and any unknown value)
        };

        var value = (HeaderCmf << 8) | (flevel << 6);
        var check = 31 - (value % 31);
        if (check == 31)
            check = 0;

        return (byte)((flevel << 6) | check);
    }

    // Exposes the underlying stream minus its last 4 bytes, which are retained as the zlib trailer
    private sealed class TrailerHoldbackStream : Stream
    {
        private const int TrailerLength = 4;

        private readonly Stream _source;

        private readonly byte[] _held = new byte[TrailerLength];
        private int _heldCount;
        private bool _isEof;

        public TrailerHoldbackStream(Stream source) => _source = source;

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush() { }

        public override long Seek(long offset, SeekOrigin origin) =>
            throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) =>
            throw new NotSupportedException();

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count == 0 || _isEof)
                return 0;

            var temp = new byte[count + TrailerLength];

            while (true)
            {
                Array.Copy(_held, temp, _heldCount);

                var read = _source.Read(temp, _heldCount, count);
                if (read <= 0)
                {
                    _isEof = true;
                    return 0;
                }

                var total = _heldCount + read;
                if (total > TrailerLength)
                {
                    var returned = total - TrailerLength;
                    Array.Copy(temp, 0, buffer, offset, returned);
                    Array.Copy(temp, returned, _held, 0, TrailerLength);
                    _heldCount = TrailerLength;
                    return returned;
                }

                Array.Copy(temp, _held, total);
                _heldCount = total;
            }
        }

        public byte[]? ReadTrailer()
        {
            var scratch = new byte[256];
            while (!_isEof)
                Read(scratch, 0, scratch.Length);

            return _heldCount == TrailerLength ? _held : null;
        }
    }
}
#endif
#endif
