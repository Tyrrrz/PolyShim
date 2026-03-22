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
            TimeSpan.FromTicks(
                checked(
                    milliseconds * TimeSpan.TicksPerMillisecond
                    + microseconds * (TimeSpan.TicksPerMillisecond / 1000)
                )
            );

        // https://learn.microsoft.com/dotnet/api/system.timespan.fromseconds#system-timespan-fromseconds(system-int64-system-int64-system-int64)
        public static TimeSpan FromSeconds(
            long seconds,
            long milliseconds = 0,
            long microseconds = 0
        ) => TimeSpan.FromMilliseconds(checked(seconds * 1000L + milliseconds), microseconds);

        // https://learn.microsoft.com/dotnet/api/system.timespan.fromminutes#system-timespan-fromminutes(system-int64-system-int64-system-int64-system-int64)
        public static TimeSpan FromMinutes(
            long minutes,
            long seconds = 0,
            long milliseconds = 0,
            long microseconds = 0
        ) => TimeSpan.FromSeconds(checked(minutes * 60L + seconds), milliseconds, microseconds);

        // https://learn.microsoft.com/dotnet/api/system.timespan.fromhours#system-timespan-fromhours(system-int64-system-int64-system-int64-system-int64-system-int64)
        public static TimeSpan FromHours(
            int hours,
            long minutes = 0,
            long seconds = 0,
            long milliseconds = 0,
            long microseconds = 0
        ) =>
            TimeSpan.FromMinutes(
                checked((long)hours * 60L + minutes),
                seconds,
                milliseconds,
                microseconds
            );

        // https://learn.microsoft.com/dotnet/api/system.timespan.fromdays#system-timespan-fromdays(system-int64-system-int64-system-int64-system-int64-system-int64-system-int64)
        public static TimeSpan FromDays(
            int days,
            int hours = 0,
            long minutes = 0,
            long seconds = 0,
            long milliseconds = 0,
            long microseconds = 0
        ) =>
            TimeSpan.FromMinutes(
                checked((long)days * 1440L + (long)hours * 60L + minutes),
                seconds,
                milliseconds,
                microseconds
            );
    }
}
#endif
