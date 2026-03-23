#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Byte
{
    extension(byte)
    {
        // https://learn.microsoft.com/dotnet/api/system.byte.tryparse#system-byte-tryparse(system-string-system-iformatprovider-system-byte@)
        public static bool TryParse(string s, IFormatProvider? provider, out byte result) =>
            byte.TryParse(s, NumberStyles.Integer, provider, out result);

        // https://learn.microsoft.com/dotnet/api/system.byte.tryparse#system-byte-tryparse(system-readonlyspan((system-char))-system-iformatprovider-system-byte@)
        public static bool TryParse(
            ReadOnlySpan<char> s,
            IFormatProvider? provider,
            out byte result
        ) => byte.TryParse(new string(s.ToArray()), provider, out result);
    }
}
#endif
