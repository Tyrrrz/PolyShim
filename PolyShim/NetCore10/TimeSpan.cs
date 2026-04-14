#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_TimeSpan
{
    extension(TimeSpan)
    {
        // https://learn.microsoft.com/dotnet/api/system.timespan.parse#system-timespan-parse(system-string-system-iformatprovider)
        public static TimeSpan Parse(string s, IFormatProvider? formatProvider) =>
            // No way to use the format provider on .NET 3.5, short of re-implementing the whole thing ourselves
            TimeSpan.Parse(s);

        // https://learn.microsoft.com/dotnet/api/system.timespan.tryparse#system-timespan-tryparse(system-string-system-iformatprovider-system-timespan@)
        public static bool TryParse(string? s, IFormatProvider? formatProvider, out TimeSpan result)
        {
            if (s is null)
            {
                result = default;
                return false;
            }

            try
            {
                result = TimeSpan.Parse(s, formatProvider);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}
#endif
