#if NETFRAMEWORK && !NET40_OR_GREATER
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.conditionalweaktable-2
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class ConditionalWeakTable<TKey, TValue>
    where TKey : class
    where TValue : class
{
    public delegate TValue CreateValueCallback(TKey key);

    private sealed class Entry
    {
        public readonly WeakReference Key;
        public readonly TValue Value;

        public Entry(WeakReference key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    private readonly List<Entry> _entries = new();
    private readonly object _lock = new();

    private void Purge()
    {
        for (var i = _entries.Count - 1; i >= 0; i--)
        {
            if (!_entries[i].Key.IsAlive)
                _entries.RemoveAt(i);
        }
    }

    public TValue GetValue(TKey key, CreateValueCallback createValueCallback)
    {
        lock (_lock)
        {
            Purge();

            foreach (var entry in _entries)
            {
                if (ReferenceEquals(entry.Key.Target, key))
                    return entry.Value;
            }

            var value = createValueCallback(key);
            _entries.Add(new Entry(new WeakReference(key), value));
            return value;
        }
    }

    public bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue? value)
    {
        lock (_lock)
        {
            Purge();

            foreach (var entry in _entries)
            {
                if (ReferenceEquals(entry.Key.Target, key))
                {
                    value = entry.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }
    }

    public void Add(TKey key, TValue value)
    {
        lock (_lock)
        {
            Purge();
            _entries.Add(new Entry(new WeakReference(key), value));
        }
    }

    public bool Remove(TKey key)
    {
        lock (_lock)
        {
            for (var i = 0; i < _entries.Count; i++)
            {
                if (ReferenceEquals(_entries[i].Key.Target, key))
                {
                    _entries.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
    }
}
#endif
