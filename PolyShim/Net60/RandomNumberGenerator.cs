#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net60_RandomNumberGenerator
{
    extension(RandomNumberGenerator rng)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getbytes#system-security-cryptography-randomnumbergenerator-getbytes(system-byte()-system-int32-system-int32)
        public void GetBytes(byte[] data, int offset, int count)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (offset + count > data.Length)
                throw new ArgumentException(
                    "The sum of offset and count exceeds the length of data."
                );

            if (count == 0)
                return;

            var buffer = new byte[count];
            rng.GetBytes(buffer);
            Array.Copy(buffer, 0, data, offset, count);
        }
    }

    extension(RandomNumberGenerator)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getint32#system-security-cryptography-randomnumbergenerator-getint32(system-int32)
        public static int GetInt32(int toExclusive)
        {
            if (toExclusive <= 0)
                throw new ArgumentOutOfRangeException(nameof(toExclusive));

            return RandomNumberGenerator.GetInt32(0, toExclusive);
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getint32#system-security-cryptography-randomnumbergenerator-getint32(system-int32-system-int32)
        public static int GetInt32(int fromInclusive, int toExclusive)
        {
            if (fromInclusive >= toExclusive)
            {
                throw new ArgumentException("fromInclusive must be less than toExclusive");
            }

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
                // Explicit cast needed for .NET Framework 3.5 where RandomNumberGenerator
                // doesn't properly expose IDisposable for using statements.
                ((IDisposable)rng).Dispose();
            }
        }
    }
}
#endif
