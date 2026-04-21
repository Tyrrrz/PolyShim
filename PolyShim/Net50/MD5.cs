#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_MD5
{
    extension(MD5)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.md5.hashdata#system-security-cryptography-md5-hashdata(system-readonlyspan((system-byte)))
        public static byte[] HashData(ReadOnlySpan<byte> source)
        {
            using var md5 = MD5.Create();
            return md5.ComputeHash(source.ToArray());
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.md5.hashdata#system-security-cryptography-md5-hashdata(system-byte())
        public static byte[] HashData(byte[] source) => MD5.HashData(source.AsSpan());

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.md5.hashdata#system-security-cryptography-md5-hashdata(system-readonlyspan((system-byte))-system-span((system-byte)))
        public static int HashData(ReadOnlySpan<byte> source, Span<byte> destination)
        {
            var hash = MD5.HashData(source);
            if (destination.Length < hash.Length)
                throw new ArgumentException("Destination is too short.", nameof(destination));

            hash.AsSpan().CopyTo(destination);
            return hash.Length;
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.md5.tryhashdata
        public static bool TryHashData(
            ReadOnlySpan<byte> source,
            Span<byte> destination,
            out int bytesWritten
        )
        {
            try
            {
                bytesWritten = MD5.HashData(source, destination);
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
