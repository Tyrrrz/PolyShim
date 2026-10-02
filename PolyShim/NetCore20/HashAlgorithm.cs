#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_HashAlgorithm
{
    extension(HashAlgorithm hashAlgorithm)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.hashalgorithm.clear
        public void Clear() => hashAlgorithm.Dispose();
    }
}
#endif
