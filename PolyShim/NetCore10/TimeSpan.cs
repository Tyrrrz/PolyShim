#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_TimeSpan
{
    // Matches the "c" (constant) standard format:
    // [-][d'.']hh':'mm':'ss['.'fffffff]
    private static readonly Regex ConstantFormatRegex = new Regex(
        @"^(-)?(?:(\d+)\.)?(\d{2}):(\d{2}):(\d{2})(?:\.(\d{1,7}))?$",
        RegexOptions.CultureInvariant | RegexOptions.Compiled
    );

    // Matches the "g" (general short) standard format:
    // [-][d':']h':'mm':'ss['.'FFFFFFF]
    private static readonly Regex GeneralShortFormatRegex = new Regex(
        @"^(-)?(?:(\d+):)?(\d{1,2}):(\d{2}):(\d{2})(?:\.(\d{1,7}))?$",
        RegexOptions.CultureInvariant | RegexOptions.Compiled
    );

    // Matches the "G" (general long) standard format:
    // [-]d':'hh':'mm':'ss'.'fffffff
    private static readonly Regex GeneralLongFormatRegex = new Regex(
        @"^(-)?(\d+):(\d{2}):(\d{2}):(\d{2})\.(\d{7})$",
        RegexOptions.CultureInvariant | RegexOptions.Compiled
    );

    private static bool TryParseStandardFormat(
        string s,
        string format,
        TimeSpanStyles styles,
        out TimeSpan result
    )
    {
        result = default;

        Regex regex;
        // AssumeNegative is only recognised by the general formats ("g"/"G"), not "c"
        bool supportsAssumeNegative;

        switch (format)
        {
            case "c":
            case "t":
            case "T":
                regex = ConstantFormatRegex;
                supportsAssumeNegative = false;
                break;
            case "g":
                regex = GeneralShortFormatRegex;
                supportsAssumeNegative = false;
                break;
            case "G":
                regex = GeneralLongFormatRegex;
                supportsAssumeNegative = false;
                break;
            default:
                return false;
        }

        var match = regex.Match(s);
        if (!match.Success)
            return false;

        var negative = match.Groups[1].Success;
        var days = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0;
        var hours = int.Parse(match.Groups[3].Value);
        var minutes = int.Parse(match.Groups[4].Value);
        var seconds = int.Parse(match.Groups[5].Value);
        var ticks = 0L;
        if (match.Groups[6].Success)
        {
            var fracStr = match.Groups[6].Value.PadRight(7, '0');
            ticks = long.Parse(fracStr);
        }

        // Validate component ranges
        if (hours >= 24 || minutes >= 60 || seconds >= 60 || ticks >= TimeSpan.TicksPerSecond)
            return false;

        var totalTicks =
            (long)days * TimeSpan.TicksPerDay
            + (long)hours * TimeSpan.TicksPerHour
            + (long)minutes * TimeSpan.TicksPerMinute
            + (long)seconds * TimeSpan.TicksPerSecond
            + ticks;

        if (negative || (supportsAssumeNegative && (styles & TimeSpanStyles.AssumeNegative) != 0))
            totalTicks = -totalTicks;

        result = new TimeSpan(totalTicks);
        return true;
    }

    private static bool TryParseExactCore(
        string? s,
        string? format,
        TimeSpanStyles styles,
        out TimeSpan result
    )
    {
        result = default;

        if (s is null || format is null)
            return false;

        // Try standard format specifiers
        if (TryParseStandardFormat(s, format, styles, out result))
            return true;

        // Custom format: build a regex from the format string and parse
        return TryParseCustomFormat(s, format, styles, out result);
    }

    private static bool TryParseCustomFormat(
        string s,
        string format,
        TimeSpanStyles styles,
        out TimeSpan result
    )
    {
        result = default;

        // Build a regex pattern from the custom format string
        var pattern = new System.Text.StringBuilder("^");
        var groupNames = new System.Collections.Generic.List<string>();
        var i = 0;

        // Handle leading optional negative sign if AssumeNegative is not set
        var hasNegSign = false;

        while (i < format.Length)
        {
            var ch = format[i];

            if (ch == '\\' && i + 1 < format.Length)
            {
                // Escaped literal
                pattern.Append(Regex.Escape(format[i + 1].ToString()));
                i += 2;
                continue;
            }

            if (ch == '\'')
            {
                // Literal string in single quotes
                i++;
                while (i < format.Length && format[i] != '\'')
                {
                    pattern.Append(Regex.Escape(format[i].ToString()));
                    i++;
                }
                i++; // skip closing quote
                continue;
            }

            // Count repeated characters
            var count = 1;
            while (i + count < format.Length && format[i + count] == ch)
                count++;

            switch (ch)
            {
                case '%':
                    // Single specifier escape – skip and let next iteration handle
                    i++;
                    continue;
                case '-':
                    pattern.Append("(-)?");
                    hasNegSign = true;
                    groupNames.Add("neg");
                    i += count;
                    break;
                case 'd':
                    pattern.Append(@"(\d+)");
                    groupNames.Add("d");
                    i += count;
                    break;
                case 'h':
                    pattern.Append(count == 1 ? @"(\d{1,2})" : @"(\d{2})");
                    groupNames.Add("h");
                    i += count;
                    break;
                case 'm':
                    pattern.Append(count == 1 ? @"(\d{1,2})" : @"(\d{2})");
                    groupNames.Add("m");
                    i += count;
                    break;
                case 's':
                    pattern.Append(count == 1 ? @"(\d{1,2})" : @"(\d{2})");
                    groupNames.Add("s");
                    i += count;
                    break;
                case 'f':
                    // Required fractional seconds (count = number of digits)
                    pattern.Append($@"(\d{{{count}}})");
                    groupNames.Add("f" + count);
                    i += count;
                    break;
                case 'F':
                    // Optional fractional seconds
                    pattern.Append($@"(\d{{1,{count}}})?");
                    groupNames.Add("F" + count);
                    i += count;
                    break;
                default:
                    pattern.Append(Regex.Escape(ch.ToString()));
                    i += count;
                    break;
            }
        }

        pattern.Append('$');

        Match match;
        try
        {
            match = Regex.Match(s, pattern.ToString(), RegexOptions.CultureInvariant);
        }
        catch
        {
            return false;
        }

        if (!match.Success)
            return false;

        var days = 0;
        var hours = 0;
        var minutes = 0;
        var seconds = 0;
        var ticks = 0L;
        var negative = false;

        for (var gi = 0; gi < groupNames.Count; gi++)
        {
            var group = match.Groups[gi + 1];
            var name = groupNames[gi];

            if (!group.Success)
                continue;

            var val = group.Value;

            if (name == "neg")
                negative = val == "-";
            else if (name == "d")
                days = int.Parse(val);
            else if (name == "h")
                hours = int.Parse(val);
            else if (name == "m")
                minutes = int.Parse(val);
            else if (name == "s")
                seconds = int.Parse(val);
            else if (name.StartsWith("f") || name.StartsWith("F"))
            {
                var padded = val.PadRight(7, '0');
                ticks = long.Parse(padded);
            }
        }

        if (!hasNegSign && (styles & TimeSpanStyles.AssumeNegative) != 0)
            negative = true;

        if (hours >= 24 || minutes >= 60 || seconds >= 60 || ticks >= TimeSpan.TicksPerSecond)
            return false;

        var totalTicks =
            (long)days * TimeSpan.TicksPerDay
            + (long)hours * TimeSpan.TicksPerHour
            + (long)minutes * TimeSpan.TicksPerMinute
            + (long)seconds * TimeSpan.TicksPerSecond
            + ticks;

        if (negative)
            totalTicks = -totalTicks;

        result = new TimeSpan(totalTicks);
        return true;
    }

    extension(TimeSpan)
    {
        // https://learn.microsoft.com/dotnet/api/system.timespan.parse#system-timespan-parse(system-string-system-iformatprovider)
        public static TimeSpan Parse(string s, IFormatProvider? formatProvider) =>
            TimeSpan.Parse(s);

        // https://learn.microsoft.com/dotnet/api/system.timespan.tryparse#system-timespan-tryparse(system-string-system-iformatprovider-system-timespan@)
        public static bool TryParse(
            string? s,
            IFormatProvider? formatProvider,
            out TimeSpan result
        ) => TimeSpan.TryParse(s, out result);

        // https://learn.microsoft.com/dotnet/api/system.timespan.parseexact#system-timespan-parseexact(system-string-system-string-system-iformatprovider)
        public static TimeSpan ParseExact(string s, string format, IFormatProvider? formatProvider)
        {
            if (!TryParseExactCore(s, format, TimeSpanStyles.None, out var result))
                throw new FormatException(
                    $"The TimeSpan string '{s}' could not be parsed because the format '{format}' is invalid."
                );
            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.timespan.parseexact#system-timespan-parseexact(system-string-system-string()-system-iformatprovider)
        public static TimeSpan ParseExact(
            string s,
            string[] formats,
            IFormatProvider? formatProvider
        )
        {
            foreach (var format in formats)
            {
                if (TryParseExactCore(s, format, TimeSpanStyles.None, out var result))
                    return result;
            }
            throw new FormatException(
                $"The TimeSpan string '{s}' could not be parsed because it does not match any of the provided formats."
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.timespan.parseexact#system-timespan-parseexact(system-string-system-string-system-iformatprovider-system-globalization-timespanstyles)
        public static TimeSpan ParseExact(
            string s,
            string format,
            IFormatProvider? formatProvider,
            TimeSpanStyles styles
        )
        {
            if (!TryParseExactCore(s, format, styles, out var result))
                throw new FormatException(
                    $"The TimeSpan string '{s}' could not be parsed because the format '{format}' is invalid."
                );
            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.timespan.parseexact#system-timespan-parseexact(system-string-system-string()-system-iformatprovider-system-globalization-timespanstyles)
        public static TimeSpan ParseExact(
            string s,
            string[] formats,
            IFormatProvider? formatProvider,
            TimeSpanStyles styles
        )
        {
            foreach (var format in formats)
            {
                if (TryParseExactCore(s, format, styles, out var result))
                    return result;
            }
            throw new FormatException(
                $"The TimeSpan string '{s}' could not be parsed because it does not match any of the provided formats."
            );
        }

        // https://learn.microsoft.com/dotnet/api/system.timespan.tryparseexact#system-timespan-tryparseexact(system-string-system-string-system-iformatprovider-system-timespan@)
        public static bool TryParseExact(
            string? s,
            string? format,
            IFormatProvider? formatProvider,
            out TimeSpan result
        ) => TryParseExactCore(s, format, TimeSpanStyles.None, out result);

        // https://learn.microsoft.com/dotnet/api/system.timespan.tryparseexact#system-timespan-tryparseexact(system-string-system-string()-system-iformatprovider-system-timespan@)
        public static bool TryParseExact(
            string? s,
            string[]? formats,
            IFormatProvider? formatProvider,
            out TimeSpan result
        )
        {
            result = default;
            if (formats is null)
                return false;
            foreach (var format in formats)
            {
                if (TryParseExactCore(s, format, TimeSpanStyles.None, out result))
                    return true;
            }
            return false;
        }

        // https://learn.microsoft.com/dotnet/api/system.timespan.tryparseexact#system-timespan-tryparseexact(system-string-system-string-system-iformatprovider-system-globalization-timespanstyles-system-timespan@)
        public static bool TryParseExact(
            string? s,
            string? format,
            IFormatProvider? formatProvider,
            TimeSpanStyles styles,
            out TimeSpan result
        ) => TryParseExactCore(s, format, styles, out result);

        // https://learn.microsoft.com/dotnet/api/system.timespan.tryparseexact#system-timespan-tryparseexact(system-string-system-string()-system-iformatprovider-system-globalization-timespanstyles-system-timespan@)
        public static bool TryParseExact(
            string? s,
            string[]? formats,
            IFormatProvider? formatProvider,
            TimeSpanStyles styles,
            out TimeSpan result
        )
        {
            result = default;
            if (formats is null)
                return false;
            foreach (var format in formats)
            {
                if (TryParseExactCore(s, format, styles, out result))
                    return true;
            }
            return false;
        }
    }
}
#endif
