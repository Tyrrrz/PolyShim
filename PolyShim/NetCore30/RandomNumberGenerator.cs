#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore30_RandomNumberGenerator
{
    extension(RandomNumberGenerator)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getint32#system-security-cryptography-randomnumbergenerator-getint32(system-int32-system-int32)
        public static int GetInt32(int fromInclusive, int toExclusive)
        {
            var range = (uint)(toExclusive - fromInclusive);
            var rng = RandomNumberGenerator.Create();
            try
            {
                // Reject values that would cause bias in the distribution.
                // This ensures uniform distribution by rejecting values in the
                // incomplete final bucket.
                var rejectionThreshold = uint.MaxValue - (uint.MaxValue % range + 1) % range;

                uint result;
                do
                {
                    var buffer = new byte[4];
                    rng.GetBytes(buffer);
                    result = BitConverter.ToUInt32(buffer, 0);
                } while (result > rejectionThreshold);

                return (int)(result % range) + fromInclusive;
            }
            finally
            {
                ((IDisposable)rng).Dispose();
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getint32#system-security-cryptography-randomnumbergenerator-getint32(system-int32)
        public static int GetInt32(int toExclusive) =>
            RandomNumberGenerator.GetInt32(0, toExclusive);
    }
}
#endif
