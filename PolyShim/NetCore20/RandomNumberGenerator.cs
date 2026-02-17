#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK && !NET46_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore20_RandomNumberGenerator
{
    extension(RandomNumberGenerator rng)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getbytes#system-security-cryptography-randomnumbergenerator-getbytes(system-byte()-system-int32-system-int32)
        public void GetBytes(byte[] data, int offset, int count)
        {
            if (count == 0)
                return;

            var buffer = new byte[count];
            rng.GetBytes(buffer);
            Array.Copy(buffer, 0, data, offset, count);
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getnonzerobytes#system-security-cryptography-randomnumbergenerator-getnonzerobytes(system-byte())
        public void GetNonZeroBytes(byte[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (byte)RandomNumberGenerator.GetInt32(1, 256);
            }
        }
    }
}
#endif
