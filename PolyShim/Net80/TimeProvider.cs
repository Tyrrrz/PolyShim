#if !FEATURE_TIMEPROVIDER
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.timeprovider
[ExcludeFromCodeCoverage]
internal abstract class TimeProvider
{
    public static TimeProvider System { get; } = new SystemTimeProvider();

    protected TimeProvider() { }

    public abstract TimeZoneInfo LocalTimeZone { get; }

    public virtual DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;

    public DateTimeOffset GetLocalNow()
    {
        var utcDateTime = GetUtcNow();
        var offset = LocalTimeZone.GetUtcOffset(utcDateTime);
        return new DateTimeOffset(utcDateTime.DateTime + offset, offset);
    }

    public virtual long GetTimestamp() => Stopwatch.GetTimestamp();

    public virtual TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp) =>
        Stopwatch.GetElapsedTime(startingTimestamp, endingTimestamp);

    public TimeSpan GetElapsedTime(long startingTimestamp) =>
        GetElapsedTime(startingTimestamp, GetTimestamp());

    public virtual ITimer CreateTimer(
        TimerCallback callback,
        object? state,
        TimeSpan dueTime,
        TimeSpan period
    ) => new SystemTimeProviderTimer(dueTime, period, callback, state);

    private sealed class SystemTimeProvider : TimeProvider
    {
        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Local;
    }

    private sealed class SystemTimeProviderTimer : ITimer
    {
        private readonly Timer _timer;

        public SystemTimeProviderTimer(
            TimeSpan dueTime,
            TimeSpan period,
            TimerCallback callback,
            object? state
        )
        {
            _timer = new Timer(callback, state, dueTime, period);
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
        public ValueTask DisposeAsync() => _timer.DisposeAsync();
#endif
    }
}
#endif
