#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Globalization;
using System.Text;

internal static partial class PolyfillExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.string.startswith#system-string-startswith(system-char)
    public static bool StartsWith(this string str, char c) => str.Length > 0 && str[0] == c;

    // https://learn.microsoft.com/en-us/dotnet/api/system.string.endswith#system-string-endswith(system-char)
    public static bool EndsWith(this string str, char c) => str.Length > 0 && str[^1] == c;

    // https://learn.microsoft.com/en-us/dotnet/api/system.string.contains#system-string-contains(system-char)
    public static bool Contains(this string str, char c) => str.IndexOf(c) >= 0;

    // https://learn.microsoft.com/en-us/dotnet/api/system.string.replace#system-string-replace(system-string-system-string-system-stringcomparison)
    public static string Replace(
        this string str,
        string oldValue,
        string? newValue,
        StringComparison comparison
    )
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

    // https://learn.microsoft.com/en-us/dotnet/api/system.string.replace#system-string-replace(system-string-system-string-system-boolean-system-globalization-cultureinfo)
    public static string Replace(
        this string str,
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

    // https://learn.microsoft.com/en-us/dotnet/api/system.string.split#system-string-split(system-char-system-int32-system-stringsplitoptions)
    public static string[] Split(
        this string str,
        char separator,
        int count,
        StringSplitOptions options = StringSplitOptions.None
    ) => str.Split(new[] { separator }, count, options);

    // https://learn.microsoft.com/en-us/dotnet/api/system.string.split#system-string-split(system-char-system-stringsplitoptions)
    public static string[] Split(
        this string str,
        char separator,
        StringSplitOptions options = StringSplitOptions.None
    ) => str.Split(new[] { separator }, options);

    // https://learn.microsoft.com/en-us/dotnet/api/system.string.split#system-string-split(system-string-system-int32-system-stringsplitoptions)
    public static string[] Split(
        this string str,
        string? separator,
        int count,
        StringSplitOptions options = StringSplitOptions.None
    ) => str.Split(new[] { separator ?? "" }, count, options);

    // https://learn.microsoft.com/en-us/dotnet/api/system.string.split#system-string-split(system-string-system-stringsplitoptions)
    public static string[] Split(
        this string str,
        string? separator,
        StringSplitOptions options = StringSplitOptions.None
    ) => str.Split(new[] { separator ?? "" }, options);
}
#endif
