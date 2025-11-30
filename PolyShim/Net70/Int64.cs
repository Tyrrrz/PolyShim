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
    extension(long)
    {
        // https://learn.microsoft.com/dotnet/api/system.int64.tryparse#system-int64-tryparse(system-string-system-iformatprovider-system-int64@)
        public static bool TryParse(string s, IFormatProvider? provider, out long result) =>
            long.TryParse(s, NumberStyles.Integer, provider, out result);
    }
}
#endif
