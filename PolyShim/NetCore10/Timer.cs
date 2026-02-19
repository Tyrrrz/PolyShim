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
internal sealed class Timer : IDisposable
{
    private readonly TimerCallback _callback;
    private readonly object? _state;
    private CancellationTokenSource _cts = new CancellationTokenSource();
    private volatile bool _disposed;

    public Timer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
        _callback = callback;
        _state = state;
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

    private void Schedule(TimeSpan dueTime, TimeSpan period)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(Timer));

        var cts = new CancellationTokenSource();
        var oldCts = Interlocked.Exchange(ref _cts, cts);
        oldCts.Cancel();
        oldCts.Dispose();

        if (dueTime == Timeout.InfiniteTimeSpan)
            return;

        _ = Task.Run(async () =>
        {
            try
            {
                if (dueTime > TimeSpan.Zero)
                    await Task.Delay(dueTime, cts.Token);

                if (cts.IsCancellationRequested)
                    return;

                _callback(_state);

                if (period == Timeout.InfiniteTimeSpan || period <= TimeSpan.Zero)
                    return;

                while (!cts.IsCancellationRequested)
                {
                    await Task.Delay(period, cts.Token);

                    if (cts.IsCancellationRequested)
                        return;

                    _callback(_state);
                }
            }
            catch (OperationCanceledException) { }
        });
    }

    public bool Change(TimeSpan dueTime, TimeSpan period)
    {
        try
        {
            Schedule(dueTime, period);
            return true;
        }
        catch
        {
            return false;
        }
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
        var cts = Interlocked.Exchange(ref _cts, new CancellationTokenSource());
        cts.Cancel();
        cts.Dispose();
    }
}
#endif
