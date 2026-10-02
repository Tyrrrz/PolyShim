#if NETSTANDARD && !NETSTANDARD1_3_OR_GREATER
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

// https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal abstract class RandomNumberGenerator : IDisposable
{
    public static RandomNumberGenerator Create() => new RandomWrapper();

    public abstract void GetBytes(byte[] data);

    protected virtual void Dispose(bool disposing) { }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
file class RandomWrapper : RandomNumberGenerator
{
    private readonly Random _random = new();

    public override void GetBytes(byte[] data) => _random.NextBytes(data);
}
#endif
