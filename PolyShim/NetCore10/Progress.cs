#if (NETFRAMEWORK && !NET45_OR_GREATER) || (NETSTANDARD && !NETSTANDARD1_3_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace System;

// https://learn.microsoft.com/dotnet/api/system.progress-1
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class Progress<T> : IProgress<T>
{
    private readonly SynchronizationContext _synchronizationContext;
    private readonly Action<T>? _handler;

    public event EventHandler<T>? ProgressChanged;

    public Progress()
    {
        _synchronizationContext = SynchronizationContext.Current ?? new();
    }

    public Progress(Action<T> handler)
        : this() => _handler = handler;

    protected virtual void OnReport(T value)
    {
        var handler = _handler;
        var progressChanged = ProgressChanged;

        if (handler is not null || progressChanged is not null)
            _synchronizationContext.Post(
                s =>
                {
                    var (v, h, e) = ((T, Action<T>?, EventHandler<T>?))s!;
                    h?.Invoke(v);
                    e?.Invoke(this, v);
                },
                (value, handler, progressChanged)
            );
    }

    void IProgress<T>.Report(T value) => OnReport(value);
}
#endif
