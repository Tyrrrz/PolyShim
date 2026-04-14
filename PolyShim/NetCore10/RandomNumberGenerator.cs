#if NETSTANDARD && !NETSTANDARD1_3_OR_GREATER
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

// Polyfill for RandomNumberGenerator class on .NET Standard < 1.3
// Note: This uses Random instead of cryptographically secure RNG as System.Security.Cryptography
// types are not available on these older platforms.
// https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator
#if !POLYFILL_COVERAGE
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

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
file class RandomWrapper : RandomNumberGenerator
{
    private readonly Random _random = new();

    public override void GetBytes(byte[] data) => _random.NextBytes(data);
}
#endif
