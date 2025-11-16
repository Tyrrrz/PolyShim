#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;

internal static partial class PolyfillExtensions
{
    extension(Math)
    {
        // https://learn.microsoft.com/dotnet/api/system.math.clamp#system-math-clamp(system-intptr-system-intptr-system-intptr)
        public static IntPtr Clamp(IntPtr value, IntPtr min, IntPtr max)
        {
            var valueAsLong = value.ToInt64();
            var minAsLong = min.ToInt64();
            var maxAsLong = max.ToInt64();

            if (valueAsLong < minAsLong)
                return min;

            if (valueAsLong > maxAsLong)
                return max;

            return value;
        }
    }
}
#endif
