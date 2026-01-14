#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;

file static class RandomEx
{
    [field: ThreadStatic]
    public static Random? Shared { get; set; }
}

internal static partial class PolyfillExtensions
{
    extension(Random random)
    {
        // https://learn.microsoft.com/dotnet/api/system.random.nextint64#system-random-nextint64(system-int64-system-int64)
        public long NextInt64(long minValue, long maxValue)
        {
            if (minValue >= maxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minValue),
                    "minValue must be less than maxValue"
                );
            }

            var range = (ulong)(maxValue - minValue);

            ulong ulongRand;
            do
            {
                var buffer = new byte[8];
                random.NextBytes(buffer);
                ulongRand = BitConverter.ToUInt64(buffer, 0);
            } while (ulongRand > ulong.MaxValue - (ulong.MaxValue % range + 1) % range);

            return (long)(ulongRand % range) + minValue;
        }

        // https://learn.microsoft.com/dotnet/api/system.random.nextint64#system-random-nextint64(system-int64)
        public long NextInt64(long maxValue) => random.NextInt64(0, maxValue);

        // https://learn.microsoft.com/dotnet/api/system.random.nextint64#system-random-nextint64
        public long NextInt64() => random.NextInt64(0, long.MaxValue);

        // https://learn.microsoft.com/dotnet/api/system.random.nextsingle
        public float NextSingle()
        {
            var buffer = new byte[4];
            random.NextBytes(buffer);
            var uintValue = BitConverter.ToUInt32(buffer, 0);

            return (uintValue >> 8) * (1.0f / (1u << 24));
        }

        // https://learn.microsoft.com/dotnet/api/system.random.shared
        public static Random Shared => RandomEx.Shared ??= new Random();
    }
}
#endif
