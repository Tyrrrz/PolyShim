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
internal static class MemberPolyfills_Net70_DateTimeOffset
{
    extension(DateTimeOffset)
    {
        // https://learn.microsoft.com/dotnet/api/system.datetimeoffset.tryparse#system-datetimeoffset-tryparse(system-string-system-iformatprovider-system-datetimeoffset@)
        public static bool TryParse(
            string? s,
            IFormatProvider? provider,
            out DateTimeOffset result
        ) => DateTimeOffset.TryParse(s, provider, DateTimeStyles.None, out result);
    }
}
#endif
