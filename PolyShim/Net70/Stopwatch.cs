#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Stopwatch
{
    extension(Stopwatch)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.stopwatch.getelapsedtime#system-diagnostics-stopwatch-getelapsedtime(system-int64-system-int64)
        public static TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp)
        {
            var tickFrequency = Stopwatch.Frequency;
            var ticks = endingTimestamp - startingTimestamp;

            if (tickFrequency == TimeSpan.TicksPerSecond)
            {
                return new TimeSpan(ticks);
            }
            else if (tickFrequency > TimeSpan.TicksPerSecond)
            {
                var ticksPerStopwatchTick = (double)tickFrequency / TimeSpan.TicksPerSecond;
                return new TimeSpan((long)(ticks / ticksPerStopwatchTick));
            }
            else
            {
                var ticksPerStopwatchTick = (double)TimeSpan.TicksPerSecond / tickFrequency;
                return new TimeSpan((long)(ticks * ticksPerStopwatchTick));
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.diagnostics.stopwatch.getelapsedtime#system-diagnostics-stopwatch-getelapsedtime(system-int64)
        public static TimeSpan GetElapsedTime(long startingTimestamp) =>
            Stopwatch.GetElapsedTime(startingTimestamp, Stopwatch.GetTimestamp());
    }
}
#endif
