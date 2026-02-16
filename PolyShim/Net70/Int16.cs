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
internal static class MemberPolyfills_Net70_Int16
{
    extension(short)
    {
        // https://learn.microsoft.com/dotnet/api/system.int16.tryparse#system-int16-tryparse(system-string-system-iformatprovider-system-int16@)
        public static bool TryParse(string s, IFormatProvider? provider, out short result) =>
            short.TryParse(s, NumberStyles.Integer, provider, out result);
    }
}
#endif
