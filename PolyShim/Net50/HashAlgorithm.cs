#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// HashAlgorithm is not available on .NET Standard < 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_HashAlgorithm
{
    extension(HashAlgorithm hashAlgorithm)
    {
        // Task infrastructure is required for async method support
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.hashalgorithm.computehashasync
        public async Task<byte[]> ComputeHashAsync(
            Stream inputStream,
            CancellationToken cancellationToken = default
        ) => await Task.Run(() => hashAlgorithm.ComputeHash(inputStream), cancellationToken);
#endif
    }
}
#endif
#endif
