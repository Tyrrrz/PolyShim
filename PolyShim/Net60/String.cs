#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;

internal static partial class PolyfillExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.string.replacelineendings#system-string-replacelineendings(system-string)
    public static string ReplaceLineEndings(this string str, string replacementText) =>
        str.Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace("\r", "\n", StringComparison.Ordinal)
            .Replace("\n", replacementText, StringComparison.Ordinal);

    // https://learn.microsoft.com/en-us/dotnet/api/system.string.replacelineendings#system-string-replacelineendings
    public static string ReplaceLineEndings(this string str) =>
        str.ReplaceLineEndings(Environment.NewLine);
}
#endif
