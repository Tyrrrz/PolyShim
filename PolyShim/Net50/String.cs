#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_String
{
    extension(string str)
    {
        // https://learn.microsoft.com/dotnet/api/system.string.indexof#system-string-indexof(system-char-system-stringcomparison)
        public int IndexOf(char value, StringComparison comparisonType) =>
            str.IndexOf(value.ToString(), comparisonType);
    }
}
#endif
