#if NETSTANDARD && !NETSTANDARD1_2_OR_GREATER
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace System.Threading;

// https://learn.microsoft.com/dotnet/api/system.threading.timer
[ExcludeFromCodeCoverage]
internal sealed class Timer(
    TimerCallback callback,
    object? state,
    TimeSpan dueTime,
    TimeSpan period
) : IDisposable
{
    private CancellationTokenSource _cts = CreateCts(callback, state, dueTime, period);
    private volatile bool _disposed;

    public Timer(TimerCallback callback, object? state, int dueTime, int period)
        : this(
            callback,
            state,
            TimeSpan.FromMilliseconds(dueTime),
            TimeSpan.FromMilliseconds(period)
        ) { }

    public Timer(TimerCallback callback, object? state, long dueTime, long period)
        : this(
            callback,
            state,
            TimeSpan.FromMilliseconds(dueTime),
            TimeSpan.FromMilliseconds(period)
        ) { }

    public Timer(TimerCallback callback)
        : this(callback, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan) { }

    private static void Start(
        TimerCallback callback,
        object? state,
        TimeSpan dueTime,
        TimeSpan period,
        CancellationToken cancellationToken
    )
    {
        if (dueTime == Timeout.InfiniteTimeSpan)
            return;

        _ = Task.Run(async () =>
        {
            try
            {
                if (dueTime > TimeSpan.Zero)
                    await Task.Delay(dueTime, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            if (cancellationToken.IsCancellationRequested)
                return;

            callback(state);

            if (period == Timeout.InfiniteTimeSpan || period == TimeSpan.Zero)
                return;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(period, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                if (cancellationToken.IsCancellationRequested)
                    return;

                callback(state);
            }
        });
    }

    private static CancellationTokenSource CreateCts(
        TimerCallback callback,
        object? state,
        TimeSpan dueTime,
        TimeSpan period
    )
    {
        if (callback is null)
            throw new ArgumentNullException(nameof(callback));
        if (dueTime != Timeout.InfiniteTimeSpan && dueTime < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(dueTime));
        if (period != Timeout.InfiniteTimeSpan && period < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(period));

        var cts = new CancellationTokenSource();
        Start(callback, state, dueTime, period, cts.Token);
        return cts;
    }

    private void Schedule(TimeSpan dueTime, TimeSpan period)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(Timer));

        if (dueTime != Timeout.InfiniteTimeSpan && dueTime < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(dueTime));
        if (period != Timeout.InfiniteTimeSpan && period < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(period));

        var cts = new CancellationTokenSource();
        Start(callback, state, dueTime, period, cts.Token);
        var oldCts = Interlocked.Exchange(ref _cts, cts);
        oldCts.Cancel();
        oldCts.Dispose();

        // Handle race where Dispose completes after the initial _disposed check
        // but before/just after the exchange: ensure the newly created CTS
        // is also cancelled and disposed so it doesn't leak or keep firing.
        if (_disposed)
        {
            cts.Cancel();
            cts.Dispose();
        }
    }

    public bool Change(TimeSpan dueTime, TimeSpan period)
    {
        Schedule(dueTime, period);
        return true;
    }

    public bool Change(int dueTime, int period) =>
        Change(TimeSpan.FromMilliseconds(dueTime), TimeSpan.FromMilliseconds(period));

    public bool Change(long dueTime, long period) =>
        Change(TimeSpan.FromMilliseconds(dueTime), TimeSpan.FromMilliseconds(period));

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        _cts.Cancel();
        _cts.Dispose();
    }
}
#endif
