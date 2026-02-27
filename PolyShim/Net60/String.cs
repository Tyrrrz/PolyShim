#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net60_String
{
    extension(string str)
    {
        // https://learn.microsoft.com/dotnet/api/system.string.replacelineendings#system-string-replacelineendings(system-string)
        public string ReplaceLineEndings(string replacementText) =>
            str.Replace("\r\n", "\n", StringComparison.Ordinal)
                .Replace("\r", "\n", StringComparison.Ordinal)
                .Replace("\n", replacementText, StringComparison.Ordinal);

        // https://learn.microsoft.com/dotnet/api/system.string.replacelineendings#system-string-replacelineendings
        public string ReplaceLineEndings() => str.ReplaceLineEndings(Environment.NewLine);
    }
}
#endif
