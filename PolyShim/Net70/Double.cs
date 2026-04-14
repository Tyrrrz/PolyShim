#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Double
{
    extension(double)
    {
        // https://learn.microsoft.com/dotnet/api/system.double.tryparse#system-double-tryparse(system-string-system-iformatprovider-system-double@)
        public static bool TryParse(string? s, IFormatProvider? provider, out double result) =>
            double.TryParse(
                s,
                NumberStyles.Float | NumberStyles.AllowThousands,
                provider,
                out result
            );

        // https://learn.microsoft.com/dotnet/api/system.double.tryparse#system-double-tryparse(system-readonlyspan((system-char))-system-iformatprovider-system-double@)
        public static bool TryParse(
            ReadOnlySpan<char> s,
            IFormatProvider? provider,
            out double result
        ) => double.TryParse(new string(s.ToArray()), provider, out result);
    }
}
#endif
