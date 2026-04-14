#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_DateTime
{
    extension(DateTime)
    {
        // https://learn.microsoft.com/dotnet/api/system.datetime.tryparse#system-datetime-tryparse(system-string-system-iformatprovider-system-datetime@)
        public static bool TryParse(string? s, IFormatProvider? provider, out DateTime result) =>
            DateTime.TryParse(s, provider, DateTimeStyles.None, out result);

        // https://learn.microsoft.com/dotnet/api/system.datetime.tryparse#system-datetime-tryparse(system-readonlyspan((system-char))-system-iformatprovider-system-datetime@)
        public static bool TryParse(
            ReadOnlySpan<char> s,
            IFormatProvider? provider,
            out DateTime result
        ) => DateTime.TryParse(new string(s.ToArray()), provider, out result);
    }
}
#endif
