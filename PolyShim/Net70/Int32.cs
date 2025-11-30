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
    extension(int)
    {
        // https://learn.microsoft.com/dotnet/api/system.int32.tryparse#system-int32-tryparse(system-string-system-iformatprovider-system-int32@)
        public static bool TryParse(string s, IFormatProvider? provider, out int result) =>
            int.TryParse(s, NumberStyles.Integer, provider, out result);
    }
}
#endif
