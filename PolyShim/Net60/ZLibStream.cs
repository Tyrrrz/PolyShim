#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK && NET45_OR_GREATER) || (NETSTANDARD && NETSTANDARD1_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

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

    private bool _headerProcessed;
    private bool _disposed;

    // Adler-32 checksum of the uncompressed data, maintained as data flows through the stream.
    private uint _adlerA = 1;
    private uint _adlerB;

    public ZLibStream(Stream stream, CompressionMode mode)
        : this(stream, mode, leaveOpen: false) { }

    public ZLibStream(Stream stream, CompressionMode mode, bool leaveOpen)
    {
        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        _baseStream = stream;
        _leaveOpen = leaveOpen;
        _mode = mode;
        _headerFlg = GetHeaderFlg(CompressionLevel.Optimal);
        _deflateStream = new DeflateStream(stream, mode, leaveOpen: true);
    }

    public ZLibStream(Stream stream, CompressionLevel compressionLevel)
        : this(stream, compressionLevel, leaveOpen: false) { }

    public ZLibStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen)
    {
        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        _baseStream = stream;
        _leaveOpen = leaveOpen;
        _mode = CompressionMode.Compress;
        _headerFlg = GetHeaderFlg(compressionLevel);
        _deflateStream = new DeflateStream(stream, compressionLevel, leaveOpen: true);
    }

    public override bool CanRead => !_disposed && _deflateStream.CanRead;

    public override bool CanWrite => !_disposed && _deflateStream.CanWrite;

    public override bool CanSeek => false;

    public override long Length => throw new NotSupportedException();

    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override void Flush()
    {
        ThrowIfDisposed();
        _deflateStream.Flush();
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override int Read(byte[] buffer, int offset, int count)
    {
        ThrowIfDisposed();

        if (_mode == CompressionMode.Decompress)
            EnsureHeaderRead();

        var bytesRead = _deflateStream.Read(buffer, offset, count);

        // The trailing Adler-32 checksum is not validated against the decompressed
        // data. This is a best-effort, API-compatible implementation, since the
        // public DeflateStream API does not expose a way to reliably determine
        // where the compressed payload ends within the underlying stream.
        return bytesRead;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        ThrowIfDisposed();

        if (_mode == CompressionMode.Compress)
        {
            EnsureHeaderWritten();
            UpdateAdler32(buffer, offset, count);
        }

        _deflateStream.Write(buffer, offset, count);
    }

    protected override void Dispose(bool disposing)
    {
        try
        {
            if (!_disposed && disposing)
            {
                _deflateStream.Dispose();

                if (_mode == CompressionMode.Compress && _headerProcessed)
                    WriteTrailer();

                if (!_leaveOpen)
                    _baseStream.Dispose();
            }
        }
        finally
        {
            _disposed = true;
            base.Dispose(disposing);
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(ZLibStream));
    }

    private void EnsureHeaderWritten()
    {
        if (_headerProcessed)
            return;

        _baseStream.WriteByte(HeaderCmf);
        _baseStream.WriteByte(_headerFlg);

        _headerProcessed = true;
    }

    private void EnsureHeaderRead()
    {
        if (_headerProcessed)
            return;

        _headerProcessed = true;

        var cmf = _baseStream.ReadByte();
        var flg = _baseStream.ReadByte();

        if (cmf < 0 || flg < 0 || (cmf & 0x0F) != 8 || ((cmf << 8) | flg) % 31 != 0)
        {
            throw new InvalidDataException("The input stream is not a valid ZLib stream.");
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
}
#endif
