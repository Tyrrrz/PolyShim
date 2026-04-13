#if NETFRAMEWORK && !NET40_OR_GREATER
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_Guid
{
    extension(Guid)
    {
        // https://learn.microsoft.com/dotnet/api/system.guid.tryparse
        public static bool TryParse(string? input, out Guid result)
        {
            if (input is null)
            {
                result = default;
                return false;
            }

            try
            {
                result = new Guid(input);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.guid.tryparseexact
        public static bool TryParseExact(string? input, string? format, out Guid result)
        {
            result = default;

            if (input is null || format is null || format.Length != 1)
                return false;

            switch (char.ToUpperInvariant(format[0]))
            {
                case 'N':
                    if (input.Length != 32 || !IsAllHex(input, 0, 32))
                        return false;
                    break;

                case 'D':
                    if (!IsGuidDFormat(input))
                        return false;
                    break;

                case 'B':
                    if (
                        input.Length != 38
                        || input[0] != '{'
                        || input[37] != '}'
                        || !IsGuidDFormat(input.Substring(1, 36))
                    )
                        return false;
                    break;

                case 'P':
                    if (
                        input.Length != 38
                        || input[0] != '('
                        || input[37] != ')'
                        || !IsGuidDFormat(input.Substring(1, 36))
                    )
                        return false;
                    break;

                case 'X':
                    return TryParseExactX(input, out result);

                default:
                    return false;
            }

            return TryParse(input, out result);
        }
    }

    private static bool IsHexChar(char c) =>
        (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');

    private static bool IsAllHex(string s, int start, int length)
    {
        if (start < 0 || start + length > s.Length)
            return false;
        for (var i = start; i < start + length; i++)
            if (!IsHexChar(s[i]))
                return false;
        return true;
    }

    private static bool IsGuidDFormat(string s) =>
        s.Length == 36
        && IsAllHex(s, 0, 8)
        && s[8] == '-'
        && IsAllHex(s, 9, 4)
        && s[13] == '-'
        && IsAllHex(s, 14, 4)
        && s[18] == '-'
        && IsAllHex(s, 19, 4)
        && s[23] == '-'
        && IsAllHex(s, 24, 12);

    private static bool TryParseHexPart(string part, int maxHexDigits, out uint value)
    {
        value = 0;

        if (part.Length < 3)
            return false;
        if (part[0] != '0' || (part[1] != 'x' && part[1] != 'X'))
            return false;

        var hexLen = part.Length - 2;
        if (hexLen == 0 || hexLen > maxHexDigits)
            return false;

        for (var i = 2; i < part.Length; i++)
        {
            value <<= 4;
            var c = part[i];
            if (c >= '0' && c <= '9')
                value |= (uint)(c - '0');
            else if (c >= 'a' && c <= 'f')
                value |= (uint)(c - 'a' + 10);
            else if (c >= 'A' && c <= 'F')
                value |= (uint)(c - 'A' + 10);
            else
                return false;
        }

        return true;
    }

    // Parses the "X" format: {0xhhhhhhhh,0xhhhh,0xhhhh,{0xhh,0xhh,0xhh,0xhh,0xhh,0xhh,0xhh,0xhh}}
    private static bool TryParseExactX(string input, out Guid result)
    {
        result = default;

        if (input.Length < 2 || input[0] != '{' || input[input.Length - 1] != '}')
            return false;

        // Remove outer braces
        var s = input.Substring(1, input.Length - 2);

        // Locate the inner brace block
        var innerStart = s.IndexOf('{');
        if (innerStart < 0 || s[s.Length - 1] != '}')
            return false;

        // Outer part ends with a comma before the inner brace, e.g. "0xhhhhhhhh,0xhhhh,0xhhhh,"
        var outerStr = s.Substring(0, innerStart);
        if (outerStr.Length == 0 || outerStr[outerStr.Length - 1] != ',')
            return false;
        outerStr = outerStr.Substring(0, outerStr.Length - 1);

        var outerParts = outerStr.Split(',');
        if (outerParts.Length != 3)
            return false;

        // Inner part, e.g. "{0xhh,0xhh,0xhh,0xhh,0xhh,0xhh,0xhh,0xhh}"
        var innerStr = s.Substring(innerStart);
        if (innerStr.Length < 2 || innerStr[0] != '{' || innerStr[innerStr.Length - 1] != '}')
            return false;

        var innerParts = innerStr.Substring(1, innerStr.Length - 2).Split(',');
        if (innerParts.Length != 8)
            return false;

        if (!TryParseHexPart(outerParts[0], 8, out var a))
            return false;
        if (!TryParseHexPart(outerParts[1], 4, out var b))
            return false;
        if (!TryParseHexPart(outerParts[2], 4, out var c))
            return false;

        var bytes = new byte[8];
        for (var i = 0; i < 8; i++)
        {
            if (!TryParseHexPart(innerParts[i], 2, out var v))
                return false;
            bytes[i] = (byte)v;
        }

        result = new Guid(
            (int)a,
            (short)b,
            (short)c,
            bytes[0],
            bytes[1],
            bytes[2],
            bytes[3],
            bytes[4],
            bytes[5],
            bytes[6],
            bytes[7]
        );
        return true;
    }
}
#endif
