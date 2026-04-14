#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Int32
{
    extension(int)
    {
        // https://learn.microsoft.com/dotnet/api/system.int32.tryparse#system-int32-tryparse(system-string-system-iformatprovider-system-int32@)
        public static bool TryParse(string? s, IFormatProvider? provider, out int result) =>
            int.TryParse(s, NumberStyles.Integer, provider, out result);

        // https://learn.microsoft.com/dotnet/api/system.int32.tryparse#system-int32-tryparse(system-readonlyspan((system-char))-system-iformatprovider-system-int32@)
        public static bool TryParse(
            ReadOnlySpan<char> s,
            IFormatProvider? provider,
            out int result
        ) => int.TryParse(new string(s.ToArray()), provider, out result);
    }
}
#endif
