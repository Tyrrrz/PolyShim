#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net70_Decimal
{
    extension(decimal)
    {
        // https://learn.microsoft.com/dotnet/api/system.decimal.tryparse#system-decimal-tryparse(system-string-system-iformatprovider-system-decimal@)
        public static bool TryParse(string s, IFormatProvider? provider, out decimal result) =>
            decimal.TryParse(s, NumberStyles.Number, provider, out result);
    }
}
#endif
