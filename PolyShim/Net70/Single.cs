#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Single
{
    extension(float)
    {
        // https://learn.microsoft.com/dotnet/api/system.single.tryparse#system-single-tryparse(system-string-system-iformatprovider-system-single@)
        public static bool TryParse(string? s, IFormatProvider? provider, out float result) =>
            float.TryParse(
                s,
                NumberStyles.Float | NumberStyles.AllowThousands,
                provider,
                out result
            );

        // https://learn.microsoft.com/dotnet/api/system.single.tryparse#system-single-tryparse(system-readonlyspan((system-char))-system-iformatprovider-system-single@)
        public static bool TryParse(
            ReadOnlySpan<char> s,
            IFormatProvider? provider,
            out float result
        ) => float.TryParse(new string(s.ToArray()), provider, out result);
    }
}
#endif
