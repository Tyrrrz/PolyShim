#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net60_RandomNumberGenerator
{
    extension(RandomNumberGenerator)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getbytes#system-security-cryptography-randomnumbergenerator-getbytes(system-int32)
        public static byte[] GetBytes(int count)
        {
            var data = new byte[count];
            RandomNumberGenerator.Fill(data);
            return data;
        }
    }
}
#endif
