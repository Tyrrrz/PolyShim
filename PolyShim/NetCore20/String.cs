#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Globalization;
using System.Text;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_String
{
    extension(string str)
    {
        // https://learn.microsoft.com/dotnet/api/system.string.startswith#system-string-startswith(system-char)
        public bool StartsWith(char c) => str.Length > 0 && str[0] == c;

        // https://learn.microsoft.com/dotnet/api/system.string.endswith#system-string-endswith(system-char)
        public bool EndsWith(char c) => str.Length > 0 && str[^1] == c;

        // https://learn.microsoft.com/dotnet/api/system.string.contains#system-string-contains(system-char)
        public bool Contains(char c) => str.IndexOf(c) >= 0;

        // https://learn.microsoft.com/dotnet/api/system.string.gethashcode#system-string-gethashcode(system-stringcomparison)
        public int GetHashCode(StringComparison comparisonType)
        {
            return comparisonType switch
            {
                StringComparison.CurrentCulture => StringComparer.CurrentCulture.GetHashCode(str),
                StringComparison.CurrentCultureIgnoreCase =>
                    StringComparer.CurrentCultureIgnoreCase.GetHashCode(str),
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
                StringComparison.InvariantCulture => StringComparer.InvariantCulture.GetHashCode(
                    str
                ),
                StringComparison.InvariantCultureIgnoreCase =>
                    StringComparer.InvariantCultureIgnoreCase.GetHashCode(str),
#endif
                StringComparison.Ordinal => StringComparer.Ordinal.GetHashCode(str),
                StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase.GetHashCode(
                    str
                ),
                _ => throw new ArgumentException(
                    "Invalid string comparison.",
                    nameof(comparisonType)
                ),
            };
        }

        // https://learn.microsoft.com/dotnet/api/system.string.replace#system-string-replace(system-string-system-string-system-stringcomparison)
        public string Replace(string oldValue, string? newValue, StringComparison comparison)
        {
            var buffer = new StringBuilder();

            var index = 0;
            var lastIndex = 0;
            while (true)
            {
                index = str.IndexOf(oldValue, index, comparison);
                if (index < 0)
                {
                    buffer.Append(str, lastIndex, str.Length - lastIndex);
                    break;
                }

                buffer.Append(str, lastIndex, index - lastIndex);
                buffer.Append(newValue);

                index += oldValue.Length;
                lastIndex = index;
            }

            return buffer.ToString();
        }

        // https://learn.microsoft.com/dotnet/api/system.string.replace#system-string-replace(system-string-system-string-system-boolean-system-globalization-cultureinfo)
        public string Replace(
            string oldValue,
            string? newValue,
            bool ignoreCase,
            CultureInfo? culture
        )
        {
            var buffer = new StringBuilder();

            var index = 0;
            var lastIndex = 0;
            while (true)
            {
                index = (culture ?? CultureInfo.CurrentCulture).CompareInfo.IndexOf(
                    str,
                    oldValue,
                    index,
                    ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None
                );

                if (index < 0)
                {
                    buffer.Append(str, lastIndex, str.Length - lastIndex);
                    break;
                }

                buffer.Append(str, lastIndex, index - lastIndex);
                buffer.Append(newValue);

                index += oldValue.Length;
                lastIndex = index;
            }

            return buffer.ToString();
        }

        // https://learn.microsoft.com/dotnet/api/system.string.split#system-string-split(system-char-system-int32-system-stringsplitoptions)
        public string[] Split(
            char separator,
            int count,
            StringSplitOptions options = StringSplitOptions.None
        ) => str.Split([separator], count, options);

        // https://learn.microsoft.com/dotnet/api/system.string.split#system-string-split(system-char-system-stringsplitoptions)
        public string[] Split(
            char separator,
            StringSplitOptions options = StringSplitOptions.None
        ) => str.Split([separator], options);

        // https://learn.microsoft.com/dotnet/api/system.string.split#system-string-split(system-string-system-int32-system-stringsplitoptions)
        public string[] Split(
            string? separator,
            int count,
            StringSplitOptions options = StringSplitOptions.None
        ) => str.Split([separator ?? ""], count, options);

        // https://learn.microsoft.com/dotnet/api/system.string.split#system-string-split(system-string-system-stringsplitoptions)
        public string[] Split(
            string? separator,
            StringSplitOptions options = StringSplitOptions.None
        ) => str.Split([separator ?? ""], options);
    }
}
#endif
