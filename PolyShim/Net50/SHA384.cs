#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

// SHA384 is not available on .NET Standard < 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_SHA384
{
    extension(SHA384)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.sha384.hashdata#system-security-cryptography-sha384-hashdata(system-readonlyspan((system-byte)))
        public static byte[] HashData(ReadOnlySpan<byte> source)
        {
            using var sha = SHA384.Create();
            return sha.ComputeHash(source.ToArray());
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.sha384.hashdata#system-security-cryptography-sha384-hashdata(system-byte())
        public static byte[] HashData(byte[] source) => SHA384.HashData(source.AsSpan());

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.sha384.hashdata#system-security-cryptography-sha384-hashdata(system-readonlyspan((system-byte))-system-span((system-byte)))
        public static int HashData(ReadOnlySpan<byte> source, Span<byte> destination)
        {
            var hash = SHA384.HashData(source);
            if (destination.Length < hash.Length)
                throw new ArgumentException("Destination is too short.", nameof(destination));

            hash.AsSpan().CopyTo(destination);
            return hash.Length;
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.sha384.tryhashdata
        public static bool TryHashData(
            ReadOnlySpan<byte> source,
            Span<byte> destination,
            out int bytesWritten
        )
        {
            try
            {
                bytesWritten = SHA384.HashData(source, destination);
                return true;
            }
            catch (ArgumentException)
            {
                bytesWritten = 0;
                return false;
            }
        }
    }
}
#endif
#endif
