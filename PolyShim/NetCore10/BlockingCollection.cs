#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace System.Collections.Concurrent;

// https://learn.microsoft.com/dotnet/api/system.collections.concurrent.blockingcollection-1
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class BlockingCollection<T> : IDisposable
{
    private readonly Queue<T> _items = new();
    private readonly object _lock = new();
    private bool _isAddingCompleted;

    public int Count
    {
        get
        {
            lock (_lock)
                return _items.Count;
        }
    }

    public bool IsAddingCompleted
    {
        get
        {
            lock (_lock)
                return _isAddingCompleted;
        }
    }

    public bool IsCompleted
    {
        get
        {
            lock (_lock)
                return _isAddingCompleted && _items.Count == 0;
        }
    }

    public void Add(T item)
    {
        lock (_lock)
        {
            if (_isAddingCompleted)
            {
                throw new InvalidOperationException(
                    "The collection has been marked as complete with regards to additions."
                );
            }

            _items.Enqueue(item);
            Monitor.Pulse(_lock);
        }
    }

    public void CompleteAdding()
    {
        lock (_lock)
        {
            _isAddingCompleted = true;
            Monitor.PulseAll(_lock);
        }
    }

    public bool TryTake([MaybeNullWhen(false)] out T item, int millisecondsTimeout)
    {
        if (millisecondsTimeout < Timeout.Infinite)
            throw new ArgumentOutOfRangeException(nameof(millisecondsTimeout));

        var stopwatch = Stopwatch.StartNew();

        lock (_lock)
        {
            while (_items.Count == 0)
            {
                // No more items will ever be added
                if (_isAddingCompleted)
                {
                    item = default;
                    return false;
                }

                if (millisecondsTimeout == Timeout.Infinite)
                {
                    Monitor.Wait(_lock);
                    continue;
                }

                var remainingMilliseconds =
                    millisecondsTimeout - (int)stopwatch.ElapsedMilliseconds;
                if (remainingMilliseconds <= 0)
                {
                    item = default;
                    return false;
                }

                Monitor.Wait(_lock, remainingMilliseconds);
            }

            item = _items.Dequeue();
            return true;
        }
    }

    public bool TryTake([MaybeNullWhen(false)] out T item, TimeSpan timeout)
    {
        var totalMilliseconds = (long)timeout.TotalMilliseconds;
        if (totalMilliseconds < Timeout.Infinite || totalMilliseconds > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(timeout));

        return TryTake(out item, (int)totalMilliseconds);
    }

    public bool TryTake([MaybeNullWhen(false)] out T item) => TryTake(out item, Timeout.Infinite);

    public T Take()
    {
        if (!TryTake(out var item))
        {
            throw new InvalidOperationException(
                "The collection argument is empty and has been marked as complete with regards to additions."
            );
        }

        return item;
    }

    public IEnumerable<T> GetConsumingEnumerable()
    {
        while (TryTake(out var item))
            yield return item;
    }

    public void Dispose() { }
}
#endif
