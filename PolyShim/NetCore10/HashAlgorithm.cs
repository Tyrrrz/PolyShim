#if NETSTANDARD && !NETSTANDARD1_3_OR_GREATER
#nullable enable
#pragma warning disable CS0436

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace System.Security.Cryptography;

// https://learn.microsoft.com/dotnet/api/system.security.cryptography.hashalgorithm
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal abstract class HashAlgorithm : IDisposable
{
    protected int HashSizeValue;
    public virtual int HashSize => HashSizeValue;

    public abstract void Initialize();

    protected abstract void HashCore(byte[] array, int ibStart, int cbSize);

    protected abstract byte[] HashFinal();

    public byte[] ComputeHash(Stream inputStream)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(4096);

        try
        {
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                HashCore(buffer, 0, bytesRead);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer, true);
        }

        var hash = HashFinal();
        Initialize();

        return hash;
    }

    public byte[] ComputeHash(byte[] buffer, int offset, int count)
    {
        using var stream = new MemoryStream(buffer, offset, count);
        return ComputeHash(stream);
    }

    public byte[] ComputeHash(byte[] buffer) => ComputeHash(buffer, 0, buffer.Length);

    protected abstract void Dispose(bool disposing);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
#endif
