#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore21_RandomNumberGenerator
{
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
                var buffer = new byte[data.Length];
                rng.GetBytes(buffer);
                buffer.CopyTo(data);
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
