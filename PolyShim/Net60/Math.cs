#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net60_Math
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
