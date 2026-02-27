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
internal static class MemberPolyfills_Net70_Double
{
    extension(double)
    {
        // https://learn.microsoft.com/dotnet/api/system.double.tryparse#system-double-tryparse(system-string-system-iformatprovider-system-double@)
        public static bool TryParse(string s, IFormatProvider? provider, out double result) =>
            double.TryParse(
                s,
                NumberStyles.Float | NumberStyles.AllowThousands,
                provider,
                out result
            );
    }
}
#endif
