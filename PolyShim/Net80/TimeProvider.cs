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

    public TimeSpan GetElapsedTime(long startingTimestamp) =>
        GetElapsedTime(startingTimestamp, GetTimestamp());

    public virtual TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp) =>
        Stopwatch.GetElapsedTime(startingTimestamp, endingTimestamp);

#if !(NETSTANDARD && !NETSTANDARD1_2_OR_GREATER)
    public ITimer CreateTimer(
        TimerCallback callback,
        object? state,
        TimeSpan dueTime,
        TimeSpan period
    ) => new SystemTimeProviderTimer(dueTime, period, callback, state);
#endif

#if FEATURE_TASK
    public virtual Task Delay(TimeSpan delay, CancellationToken cancellationToken = default)
    {
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

    public CancellationTokenSource CreateCancellationTokenSource(TimeSpan delay)
    {
        var infiniteTimeSpan = TimeSpan.FromMilliseconds(-1);

        if (delay == infiniteTimeSpan)
            return new CancellationTokenSource();

#if NET45_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NETCOREAPP
        return new CancellationTokenSource(delay);
#else
        // CancellationTokenSource(int) constructor added in .NET 4.5
        // For older frameworks, just return a plain instance
        return new CancellationTokenSource();
#endif
    }
#endif

    private sealed class SystemTimeProvider : TimeProvider
    {
        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Local;
    }

#if !(NETSTANDARD && !NETSTANDARD1_2_OR_GREATER)
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
#endif
}
#endif
