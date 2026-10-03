#if NETSTANDARD && !NETSTANDARD1_2_OR_GREATER
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace System.Threading;

// https://learn.microsoft.com/dotnet/api/system.threading.timer
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class Timer(TimerCallback callback, object? state) : IDisposable
{
    private CancellationTokenSource? _cts;
    private volatile bool _isDisposed;

    public Timer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
        : this(callback ?? throw new ArgumentNullException(nameof(callback)), state)
    {
        Schedule(dueTime, period);
    }

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

    private void Schedule(TimeSpan dueTime, TimeSpan period)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        if (dueTime != Timeout.InfiniteTimeSpan && dueTime < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(dueTime));
        if (period != Timeout.InfiniteTimeSpan && period < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(period));

        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var oldCts = Interlocked.Exchange(ref _cts, cts);

        try
        {
            oldCts?.Cancel();
        }
        catch (ObjectDisposedException) { }

        oldCts?.Dispose();

        Start(callback, state, dueTime, period, token);

        // Handle race where Dispose completes after the initial _isDisposed check
        // but before/just after the exchange: ensure the newly created CTS
        // is also cancelled and disposed so it doesn't leak or keep firing.
        if (_isDisposed)
        {
            try
            {
                cts.Cancel();
            }
            catch (ObjectDisposedException) { }

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
        if (_isDisposed)
            return;

        _isDisposed = true;
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
#endif
