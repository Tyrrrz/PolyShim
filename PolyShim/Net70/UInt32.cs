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
internal static class MemberPolyfills_Net70_UInt32
{
    extension(uint)
    {
        // https://learn.microsoft.com/dotnet/api/system.uint32.tryparse#system-uint32-tryparse(system-string-system-iformatprovider-system-uint32@)
        public static bool TryParse(string s, IFormatProvider? provider, out uint result) =>
            uint.TryParse(s, NumberStyles.Integer, provider, out result);
    }
}
#endif
