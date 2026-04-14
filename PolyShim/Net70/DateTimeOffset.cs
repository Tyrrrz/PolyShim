#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_DateTimeOffset
{
    extension(DateTimeOffset)
    {
        // https://learn.microsoft.com/dotnet/api/system.datetimeoffset.tryparse#system-datetimeoffset-tryparse(system-string-system-iformatprovider-system-datetimeoffset@)
        public static bool TryParse(
            string? s,
            IFormatProvider? provider,
            out DateTimeOffset result
        ) => DateTimeOffset.TryParse(s, provider, DateTimeStyles.None, out result);

        // https://learn.microsoft.com/dotnet/api/system.datetimeoffset.tryparse#system-datetimeoffset-tryparse(system-readonlyspan((system-char))-system-iformatprovider-system-datetimeoffset@)
        public static bool TryParse(
            ReadOnlySpan<char> s,
            IFormatProvider? provider,
            out DateTimeOffset result
        ) => DateTimeOffset.TryParse(new string(s.ToArray()), provider, out result);
    }
}
#endif
