#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Char
{
    extension(char)
    {
        // https://learn.microsoft.com/dotnet/api/system.char.isbetween
        public static bool IsBetween(char c, char minInclusive, char maxInclusive) =>
            (uint)(c - minInclusive) <= (uint)(maxInclusive - minInclusive);

        // https://learn.microsoft.com/dotnet/api/system.char.isasciidigit
        public static bool IsAsciiDigit(char c) => char.IsBetween(c, '0', '9');

        // https://learn.microsoft.com/dotnet/api/system.char.isasciihexdigit
        public static bool IsAsciiHexDigit(char c) =>
            char.IsAsciiHexDigitUpper(c) || char.IsAsciiHexDigitLower(c);

        // https://learn.microsoft.com/dotnet/api/system.char.isasciihexdigitlower
        public static bool IsAsciiHexDigitLower(char c) =>
            char.IsBetween(c, 'a', 'f') || char.IsAsciiDigit(c);

        // https://learn.microsoft.com/dotnet/api/system.char.isasciihexdigitupper
        public static bool IsAsciiHexDigitUpper(char c) =>
            char.IsBetween(c, 'A', 'F') || char.IsAsciiDigit(c);

        // https://learn.microsoft.com/dotnet/api/system.char.isasciiletter
        public static bool IsAsciiLetter(char c) =>
            char.IsAsciiLetterUpper(c) || char.IsAsciiLetterLower(c);

        // https://learn.microsoft.com/dotnet/api/system.char.isasciiletterlower
        public static bool IsAsciiLetterLower(char c) => char.IsBetween(c, 'a', 'z');

        // https://learn.microsoft.com/dotnet/api/system.char.isasciiletterordigit
        public static bool IsAsciiLetterOrDigit(char c) =>
            char.IsAsciiLetter(c) || char.IsAsciiDigit(c);

        // https://learn.microsoft.com/dotnet/api/system.char.isasciiletterupper
        public static bool IsAsciiLetterUpper(char c) => char.IsBetween(c, 'A', 'Z');
    }
}
#endif
