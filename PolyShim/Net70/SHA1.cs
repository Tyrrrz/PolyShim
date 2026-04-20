#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

// SHA1 is not available on .NET Standard < 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_SHA1
{
    extension(SHA1)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.sha1.hashdata#system-security-cryptography-sha1-hashdata(system-io-stream)
        public static byte[] HashData(Stream source)
        {
            using var sha = SHA1.Create();
            return sha.ComputeHash(source);
        }

        // Task infrastructure is required for async method support
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.sha1.hashdataasync#system-security-cryptography-sha1-hashdataasync(system-io-stream-system-threading-cancellationtoken)
        public static async ValueTask<byte[]> HashDataAsync(
            Stream source,
            CancellationToken cancellationToken = default
        )
        {
            using var ms = new MemoryStream();
            await source.CopyToAsync(ms, cancellationToken).ConfigureAwait(false);
            ms.Position = 0;
            return SHA1.HashData(ms);
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.sha1.hashdataasync#system-security-cryptography-sha1-hashdataasync(system-io-stream-system-memory((system-byte))-system-threading-cancellationtoken)
        public static async ValueTask<int> HashDataAsync(
            Stream source,
            Memory<byte> destination,
            CancellationToken cancellationToken = default
        )
        {
            var hash = await SHA1
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
