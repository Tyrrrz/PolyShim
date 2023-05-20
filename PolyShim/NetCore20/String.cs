﻿#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;

internal static partial class PolyfillExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.string.startswith#system-string-startswith(system-char)
    public static bool StartsWith(this string str, char c) =>
        str.Length > 0 && str[0] == c;

    // https://learn.microsoft.com/en-us/dotnet/api/system.string.endswith#system-string-endswith(system-char)
    public static bool EndsWith(this string str, char c) =>
        str.Length > 0 && str[^1] == c;

    // https://learn.microsoft.com/en-us/dotnet/api/system.string.contains#system-string-contains(system-char)
    public static bool Contains(this string str, char c) =>
        str.IndexOf(c) >= 0;

    // https://learn.microsoft.com/en-us/dotnet/api/system.string.split#system-string-split(system-char-system-stringsplitoptions)
    public static string[] Split(
        this string str,
        char separator,
        StringSplitOptions options = StringSplitOptions.None) =>
        str.Split(new[] { separator }, options);
}
#endif