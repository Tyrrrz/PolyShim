#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_String
{
    extension(string str)
    {
        // https://learn.microsoft.com/dotnet/api/system.string.contains#system-string-contains(system-char-system-stringcomparison)
        public bool Contains(char c, StringComparison comparison) =>
            str.Contains(c.ToString(), comparison);

        // https://learn.microsoft.com/dotnet/api/system.string.contains#system-string-contains(system-string-system-stringcomparison)
        public bool Contains(string sub, StringComparison comparison) =>
            str.IndexOf(sub, comparison) >= 0;
    }
}
#endif
