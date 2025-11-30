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
    extension(sbyte)
    {
        // https://learn.microsoft.com/dotnet/api/system.sbyte.tryparse#system-sbyte-tryparse(system-string-system-iformatprovider-system-sbyte@)
        public static bool TryParse(string s, IFormatProvider? provider, out sbyte result) =>
            sbyte.TryParse(s, NumberStyles.Integer, provider, out result);
    }
}
#endif
