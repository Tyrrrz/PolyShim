#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Globalization;

internal static partial class PolyfillExtensions
{
    extension(decimal)
    {
        // https://learn.microsoft.com/dotnet/api/system.decimal.tryparse#system-decimal-tryparse(system-string-system-iformatprovider-system-decimal@)
        public static bool TryParse(string s, IFormatProvider? provider, out decimal result) =>
            decimal.TryParse(s, NumberStyles.Number, provider, out result);
    }
}
#endif
