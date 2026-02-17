#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore21_RandomNumberGenerator
{
    extension(RandomNumberGenerator rng)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getbytes#system-security-cryptography-randomnumbergenerator-getbytes(system-span((system-byte)))
        public void GetBytes(Span<byte> data)
        {
            if (data.Length == 0)
                return;

            var buffer = ArrayPool<byte>.Shared.Rent(data.Length);
            try
            {
                rng.GetBytes(buffer);
                buffer.AsSpan(0, data.Length).CopyTo(data);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getnonzerobytes#system-security-cryptography-randomnumbergenerator-getnonzerobytes(system-span((system-byte)))
        public void GetNonZeroBytes(Span<byte> data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (byte)RandomNumberGenerator.GetInt32(1, 256);
            }
        }
    }

    extension(RandomNumberGenerator)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.fill
        public static void Fill(Span<byte> data)
        {
            if (data.Length == 0)
                return;

            var rng = RandomNumberGenerator.Create();
            try
            {
                rng.GetBytes(data);
            }
            finally
            {
                // Explicit cast needed for .NET Framework 3.5 where RandomNumberGenerator
                // doesn't properly expose IDisposable for using statements.
                ((IDisposable)rng).Dispose();
            }
        }
    }
}
#endif
