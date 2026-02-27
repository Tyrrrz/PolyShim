#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Globalization;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_UInt64
{
    extension(ulong)
    {
        // https://learn.microsoft.com/dotnet/api/system.uint64.tryparse#system-uint64-tryparse(system-string-system-iformatprovider-system-uint64@)
        public static bool TryParse(string s, IFormatProvider? provider, out ulong result) =>
            ulong.TryParse(s, NumberStyles.Integer, provider, out result);
    }
}
#endif
