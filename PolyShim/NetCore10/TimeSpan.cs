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

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_TimeSpan
{
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
            try
            {
                return TimeSpan.Parse(s);
            }
            catch (Exception ex)
            {
                throw new FormatException(
                    $"The TimeSpan string '{s}' could not be parsed using the format '{format}'.",
                    ex
                );
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.timespan.parseexact#system-timespan-parseexact(system-string-system-string()-system-iformatprovider)
        public static TimeSpan ParseExact(
            string s,
            string[] formats,
            IFormatProvider? formatProvider
        )
        {
            try
            {
                return TimeSpan.Parse(s);
            }
            catch (Exception ex)
            {
                throw new FormatException(
                    $"The TimeSpan string '{s}' could not be parsed because it does not match any of the provided formats.",
                    ex
                );
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.timespan.parseexact#system-timespan-parseexact(system-string-system-string-system-iformatprovider-system-globalization-timespanstyles)
        public static TimeSpan ParseExact(
            string s,
            string format,
            IFormatProvider? formatProvider,
            TimeSpanStyles styles
        )
        {
            TimeSpan result;
            try
            {
                result = TimeSpan.Parse(s);
            }
            catch (Exception ex)
            {
                throw new FormatException(
                    $"The TimeSpan string '{s}' could not be parsed using the format '{format}'.",
                    ex
                );
            }

            return (styles & TimeSpanStyles.AssumeNegative) != 0 && result >= TimeSpan.Zero
                ? -result
                : result;
        }

        // https://learn.microsoft.com/dotnet/api/system.timespan.parseexact#system-timespan-parseexact(system-string-system-string()-system-iformatprovider-system-globalization-timespanstyles)
        public static TimeSpan ParseExact(
            string s,
            string[] formats,
            IFormatProvider? formatProvider,
            TimeSpanStyles styles
        )
        {
            TimeSpan result;
            try
            {
                result = TimeSpan.Parse(s);
            }
            catch (Exception ex)
            {
                throw new FormatException(
                    $"The TimeSpan string '{s}' could not be parsed because it does not match any of the provided formats.",
                    ex
                );
            }

            return (styles & TimeSpanStyles.AssumeNegative) != 0 && result >= TimeSpan.Zero
                ? -result
                : result;
        }

        // https://learn.microsoft.com/dotnet/api/system.timespan.tryparseexact#system-timespan-tryparseexact(system-string-system-string-system-iformatprovider-system-timespan@)
        public static bool TryParseExact(
            string? s,
            string? format,
            IFormatProvider? formatProvider,
            out TimeSpan result
        ) => TimeSpan.TryParse(s, out result);

        // https://learn.microsoft.com/dotnet/api/system.timespan.tryparseexact#system-timespan-tryparseexact(system-string-system-string()-system-iformatprovider-system-timespan@)
        public static bool TryParseExact(
            string? s,
            string[]? formats,
            IFormatProvider? formatProvider,
            out TimeSpan result
        ) => TimeSpan.TryParse(s, out result);

        // https://learn.microsoft.com/dotnet/api/system.timespan.tryparseexact#system-timespan-tryparseexact(system-string-system-string-system-iformatprovider-system-globalization-timespanstyles-system-timespan@)
        public static bool TryParseExact(
            string? s,
            string? format,
            IFormatProvider? formatProvider,
            TimeSpanStyles styles,
            out TimeSpan result
        )
        {
            if (!TimeSpan.TryParse(s, out result))
                return false;

            if ((styles & TimeSpanStyles.AssumeNegative) != 0 && result >= TimeSpan.Zero)
                result = -result;

            return true;
        }

        // https://learn.microsoft.com/dotnet/api/system.timespan.tryparseexact#system-timespan-tryparseexact(system-string-system-string()-system-iformatprovider-system-globalization-timespanstyles-system-timespan@)
        public static bool TryParseExact(
            string? s,
            string[]? formats,
            IFormatProvider? formatProvider,
            TimeSpanStyles styles,
            out TimeSpan result
        )
        {
            if (!TimeSpan.TryParse(s, out result))
                return false;

            if ((styles & TimeSpanStyles.AssumeNegative) != 0 && result >= TimeSpan.Zero)
                result = -result;

            return true;
        }
    }
}
#endif
