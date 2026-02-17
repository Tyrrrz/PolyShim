#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
using System.Security.Cryptography;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore21_RandomNumberGenerator
{
    extension(RandomNumberGenerator)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.fill
        public static void Fill(Span<byte> data)
        {
            // Fast-path for empty spans to avoid unnecessary allocations
            if (data.Length == 0)
                return;

            var buffer = new byte[data.Length];
            using var rng = (IDisposable)RandomNumberGenerator.Create();
            ((RandomNumberGenerator)rng).GetBytes(buffer);
            buffer.CopyTo(data);
        }
    }
}
#endif
#endif
