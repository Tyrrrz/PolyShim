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
    extension(ulong)
    {
        // https://learn.microsoft.com/dotnet/api/system.uint64.tryparse#system-uint64-tryparse(system-string-system-iformatprovider-system-uint64@)
        public static bool TryParse(string s, IFormatProvider? provider, out ulong result) =>
            ulong.TryParse(s, NumberStyles.Integer, provider, out result);
    }
}
#endif
