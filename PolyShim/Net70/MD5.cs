#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_MD5
{
    extension(MD5)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.md5.hashdata#system-security-cryptography-md5-hashdata(system-io-stream)
        public static byte[] HashData(Stream source)
        {
            using var md5 = MD5.Create();
            return md5.ComputeHash(source);
        }

        // Task infrastructure is required for async method support
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.md5.hashdataasync#system-security-cryptography-md5-hashdataasync(system-io-stream-system-threading-cancellationtoken)
        public static async ValueTask<byte[]> HashDataAsync(
            Stream source,
            CancellationToken cancellationToken = default
        )
        {
            using var ms = new MemoryStream();
            await source.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);
            ms.Position = 0;
            return MD5.HashData(ms);
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.md5.hashdataasync#system-security-cryptography-md5-hashdataasync(system-io-stream-system-memory((system-byte))-system-threading-cancellationtoken)
        public static async ValueTask<int> HashDataAsync(
            Stream source,
            Memory<byte> destination,
            CancellationToken cancellationToken = default
        )
        {
            var hash = await MD5
                .HashDataAsync(source, cancellationToken)
                .ConfigureAwait(false);
            if (destination.Length < hash.Length)
                throw new ArgumentException("Destination is too short.", nameof(destination));

            hash.AsMemory().CopyTo(destination);
            return hash.Length;
        }
#endif
    }
}
#endif
#endif
