#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK && NET45_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Security.Cryptography;

internal static partial class PolyfillExtensions
{
    extension(RandomNumberGenerator)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.fill
        public static void Fill(Span<byte> data)
        {
            var buffer = new byte[data.Length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);
            buffer.CopyTo(data);
        }
    }
}
#endif
