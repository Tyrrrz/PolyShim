#if !FEATURE_TIMEPROVIDER
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using SystemThreading = System.Threading;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.timeprovider
[ExcludeFromCodeCoverage]
internal abstract class TimeProvider
{
    public static TimeProvider System { get; } = new SystemTimeProvider();

    protected TimeProvider() { }

    public virtual DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;

    public DateTimeOffset GetLocalNow()
    {
        var utcDateTime = GetUtcNow();
        var offset = LocalTimeZone.GetUtcOffset(utcDateTime);
        return new DateTimeOffset(utcDateTime.DateTime + offset, offset);
    }

    public abstract TimeZoneInfo LocalTimeZone { get; }

    public virtual long GetTimestamp() => Stopwatch.GetTimestamp();

    public TimeSpan GetElapsedTime(long startingTimestamp) =>
        GetElapsedTime(startingTimestamp, GetTimestamp());

    public virtual TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp)
    {
        // Stopwatch.GetElapsedTime was added in .NET 7, so we need to calculate it manually
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

    // Timer and TimerCallback are not available on .NET Standard 1.0 and 1.1
#if !(NETSTANDARD && !NETSTANDARD1_2_OR_GREATER)
    public SystemThreading.ITimer CreateTimer(
        SystemThreading.TimerCallback callback,
        object? state,
        TimeSpan dueTime,
        TimeSpan period
    ) => new SystemTimeProviderTimer(dueTime, period, callback, state);
#endif

    // Task.Delay, Timeout.InfiniteTimeSpan, and CancellationTokenSource(TimeSpan) require .NET 4.5+
#if FEATURE_TASK && (NETSTANDARD1_3_OR_GREATER || NETCOREAPP || NET45_OR_GREATER)
    public virtual Task Delay(
        TimeSpan delay,
        SystemThreading.CancellationToken cancellationToken = default
    )
    {
        if (delay < TimeSpan.Zero && delay != SystemThreading.Timeout.InfiniteTimeSpan)
            throw new ArgumentOutOfRangeException(nameof(delay));

        if (cancellationToken.IsCancellationRequested)
        {
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetCanceled();
            return tcs.Task;
        }

        if (delay == TimeSpan.Zero)
            return Task.CompletedTask;

        return Task.Delay(delay, cancellationToken);
    }

    public SystemThreading.CancellationTokenSource CreateCancellationTokenSource(TimeSpan delay)
    {
        if (delay < TimeSpan.Zero && delay != SystemThreading.Timeout.InfiniteTimeSpan)
            throw new ArgumentOutOfRangeException(nameof(delay));

        if (delay == SystemThreading.Timeout.InfiniteTimeSpan)
            return new SystemThreading.CancellationTokenSource();

        return new SystemThreading.CancellationTokenSource(delay);
    }
#endif

    private sealed class SystemTimeProvider : TimeProvider
    {
        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Local;
    }

    // Timer and TimerCallback are not available on .NET Standard 1.0 and 1.1
#if !(NETSTANDARD && !NETSTANDARD1_2_OR_GREATER)
    private sealed class SystemTimeProviderTimer : SystemThreading.ITimer
    {
        private readonly SystemThreading.Timer _timer;

        public SystemTimeProviderTimer(
            TimeSpan dueTime,
            TimeSpan period,
            SystemThreading.TimerCallback callback,
            object? state
        )
        {
            _timer = new SystemThreading.Timer(callback, state, dueTime, period);
        }

        public bool Change(TimeSpan dueTime, TimeSpan period)
        {
            try
            {
                return _timer.Change(dueTime, period);
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

#if FEATURE_ASYNCINTERFACES
        public ValueTask DisposeAsync()
        {
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            return _timer.DisposeAsync();
#else
            _timer.Dispose();
            return default;
#endif
        }
#endif
    }
#endif
}
#endif
