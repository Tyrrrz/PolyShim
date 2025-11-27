#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Text.RegularExpressions;

internal static partial class PolyfillExtensions
{
    extension(Regex regex)
    {
        // https://learn.microsoft.com/dotnet/api/system.text.regularexpressions.regex.count#system-text-regularexpressions-regex-count(system-string)
        public int Count(string input)
        {
            var count = 0;
            var match = regex.Match(input);
            while (match.Success)
            {
                count++;
                match = match.NextMatch();
            }

            return count;
        }
    }

    extension(Regex)
    {
        // https://learn.microsoft.com/dotnet/api/system.text.regularexpressions.regex.count#system-text-regularexpressions-regex-count(system-string-system-string)
        public static int Count(string input, string pattern) =>
            Regex.Matches(input, pattern).Count;

        // https://learn.microsoft.com/dotnet/api/system.text.regularexpressions.regex.count#system-text-regularexpressions-regex-count(system-string-system-string-system-text-regularexpressions-regexoptions)
        public static int Count(string input, string pattern, RegexOptions options) =>
            Regex.Matches(input, pattern, options).Count;

#if !NETFRAMEWORK || NET45_OR_GREATER
        // https://learn.microsoft.com/dotnet/api/system.text.regularexpressions.regex.count#system-text-regularexpressions-regex-count(system-string-system-string-system-text-regularexpressions-regexoptions-system-timespan)
        public static int Count(
            string input,
            string pattern,
            RegexOptions options,
            TimeSpan matchTimeout
        ) => Regex.Matches(input, pattern, options, matchTimeout).Count;
#endif
    }
}
#endif
