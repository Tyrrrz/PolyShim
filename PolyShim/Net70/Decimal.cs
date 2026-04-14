#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Decimal
{
    extension(decimal)
    {
        // https://learn.microsoft.com/dotnet/api/system.decimal.tryparse#system-decimal-tryparse(system-string-system-iformatprovider-system-decimal@)
        public static bool TryParse(string? s, IFormatProvider? provider, out decimal result) =>
            decimal.TryParse(s, NumberStyles.Number, provider, out result);

        // https://learn.microsoft.com/dotnet/api/system.decimal.tryparse#system-decimal-tryparse(system-readonlyspan((system-char))-system-iformatprovider-system-decimal@)
        public static bool TryParse(
            ReadOnlySpan<char> s,
            IFormatProvider? provider,
            out decimal result
        ) => decimal.TryParse(new string(s.ToArray()), provider, out result);
    }
}
#endif
