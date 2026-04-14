#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Int64
{
    extension(long)
    {
        // https://learn.microsoft.com/dotnet/api/system.int64.tryparse#system-int64-tryparse(system-string-system-iformatprovider-system-int64@)
        public static bool TryParse(string? s, IFormatProvider? provider, out long result) =>
            long.TryParse(s, NumberStyles.Integer, provider, out result);

        // https://learn.microsoft.com/dotnet/api/system.int64.tryparse#system-int64-tryparse(system-readonlyspan((system-char))-system-iformatprovider-system-int64@)
        public static bool TryParse(
            ReadOnlySpan<char> s,
            IFormatProvider? provider,
            out long result
        ) => long.TryParse(new string(s.ToArray()), provider, out result);
    }
}
#endif
