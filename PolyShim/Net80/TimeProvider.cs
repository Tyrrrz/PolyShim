#if !FEATURE_TIMEPROVIDER && ((NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD))
// Only include TimeProvider if we have Task support and are on .NET Standard 2.0+, .NET Core 2.0+, or .NET Framework 4.6.1+
// This matches the support level of Microsoft.Bcl.TimeProvider compatibility package
#if FEATURE_TASK && (NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET461_OR_GREATER)
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
    private static readonly TimeProvider s_system = new SystemTimeProvider();
    private static readonly TimeSpan s_infiniteTimeSpan = SystemThreading.Timeout.InfiniteTimeSpan;

    public static TimeProvider System => s_system;

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

    public SystemThreading.ITimer CreateTimer(
        SystemThreading.TimerCallback callback,
        object? state,
        TimeSpan dueTime,
        TimeSpan period
    )
    {
        if (callback is null)
            throw new ArgumentNullException(nameof(callback));

        return new SystemTimeProviderTimer(dueTime, period, callback, state);
    }

    public virtual Task Delay(
        TimeSpan delay,
        SystemThreading.CancellationToken cancellationToken = default
    )
    {
        if (delay < TimeSpan.Zero && delay != s_infiniteTimeSpan)
            throw new ArgumentOutOfRangeException(nameof(delay));

        if (cancellationToken.IsCancellationRequested)
        {
#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
            return Task.FromCanceled(cancellationToken);
#else
            var tcs = new TaskCompletionSource<bool>();
            tcs.SetCanceled();
            return tcs.Task;
#endif
        }

        if (delay == TimeSpan.Zero)
        {
#if NETSTANDARD2_0 || NET461 || NET462
            return Task.FromResult(false);
#else
            return Task.CompletedTask;
#endif
        }

        return Task.Delay(delay, cancellationToken);
    }

    public SystemThreading.CancellationTokenSource CreateCancellationTokenSource(TimeSpan delay)
    {
        if (delay < TimeSpan.Zero && delay != s_infiniteTimeSpan)
            throw new ArgumentOutOfRangeException(nameof(delay));

        if (delay == s_infiniteTimeSpan)
        {
            return new SystemThreading.CancellationTokenSource();
        }

        return new SystemThreading.CancellationTokenSource(delay);
    }

    private sealed class SystemTimeProvider : TimeProvider
    {
        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Local;
    }

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
#elif FEATURE_VALUETASK
        public ValueTask DisposeAsync()
        {
            _timer.Dispose();
            return default;
        }
#endif
    }
}
#endif
#endif
