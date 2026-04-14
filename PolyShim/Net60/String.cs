#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_EXCLUDE_COVERAGE
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
