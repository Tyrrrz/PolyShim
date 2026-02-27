#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Single
{
    extension(float)
    {
        // https://learn.microsoft.com/dotnet/api/system.single.tryparse#system-single-tryparse(system-string-system-iformatprovider-system-single@)
        public static bool TryParse(string s, IFormatProvider? provider, out float result) =>
            float.TryParse(
                s,
                NumberStyles.Float | NumberStyles.AllowThousands,
                provider,
                out result
            );
    }
}
#endif
