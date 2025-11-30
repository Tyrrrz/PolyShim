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
    extension(ushort)
    {
        // https://learn.microsoft.com/dotnet/api/system.uint16.tryparse#system-uint16-tryparse(system-string-system-iformatprovider-system-uint16@)
        public static bool TryParse(string s, IFormatProvider? provider, out ushort result) =>
            ushort.TryParse(s, NumberStyles.Integer, provider, out result);
    }
}
#endif
