#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net90_TimeSpan
{
    extension(TimeSpan)
    {
        // https://learn.microsoft.com/dotnet/api/system.timespan.frommilliseconds#system-timespan-frommilliseconds(system-int64-system-int64)
        public static TimeSpan FromMilliseconds(long milliseconds, long microseconds) =>
            TimeSpan.FromMilliseconds(milliseconds + microseconds / 1_000.0);

        // https://learn.microsoft.com/dotnet/api/system.timespan.fromseconds#system-timespan-fromseconds(system-int64-system-int64-system-int64)
        public static TimeSpan FromSeconds(
            long seconds,
            long milliseconds = 0,
            long microseconds = 0
        ) => TimeSpan.FromSeconds(seconds + milliseconds / 1_000.0 + microseconds / 1_000_000.0);

        // https://learn.microsoft.com/dotnet/api/system.timespan.fromminutes#system-timespan-fromminutes(system-int64-system-int64-system-int64-system-int64)
        public static TimeSpan FromMinutes(
            long minutes,
            long seconds = 0,
            long milliseconds = 0,
            long microseconds = 0
        ) =>
            TimeSpan.FromMinutes(
                minutes + seconds / 60.0 + milliseconds / 60_000.0 + microseconds / 60_000_000.0
            );

        // https://learn.microsoft.com/dotnet/api/system.timespan.fromhours#system-timespan-fromhours(system-int64-system-int64-system-int64-system-int64-system-int64)
        public static TimeSpan FromHours(
            long hours,
            long minutes = 0,
            long seconds = 0,
            long milliseconds = 0,
            long microseconds = 0
        ) =>
            TimeSpan.FromHours(
                hours
                    + minutes / 60.0
                    + seconds / 3_600.0
                    + milliseconds / 3_600_000.0
                    + microseconds / 3_600_000_000.0
            );

        // https://learn.microsoft.com/dotnet/api/system.timespan.fromdays#system-timespan-fromdays(system-int64-system-int64-system-int64-system-int64-system-int64-system-int64)
        public static TimeSpan FromDays(
            long days,
            long hours = 0,
            long minutes = 0,
            long seconds = 0,
            long milliseconds = 0,
            long microseconds = 0
        ) =>
            TimeSpan.FromDays(
                days
                    + hours / 24.0
                    + minutes / 1_440.0
                    + seconds / 86_400.0
                    + milliseconds / 86_400_000.0
                    + microseconds / 86_400_000_000.0
            );
    }
}
#endif
