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

    // EventHandler<TEventArgs> on .NET Framework 3.5/4.0 requires TEventArgs : EventArgs,
    // so the ProgressChanged event is only available on platforms where that constraint is absent.
#if !NETFRAMEWORK || NET45_OR_GREATER
    public event EventHandler<T>? ProgressChanged;
#endif

    public Progress()
    {
        _synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
    }

    public Progress(Action<T> handler)
        : this()
    {
        _handler = handler;
    }

    protected virtual void OnReport(T value)
    {
        var handler = _handler;
#if !NETFRAMEWORK || NET45_OR_GREATER
        var progressChanged = ProgressChanged;
#else
        Action<T>? progressChanged = null;
#endif
        if (handler != null || progressChanged != null)
        {
            _synchronizationContext.Post(InvokeHandlers, value);
        }
    }

    void IProgress<T>.Report(T value) => OnReport(value);

    private void InvokeHandlers(object? state)
    {
        var value = (T)state!;
        _handler?.Invoke(value);
#if !NETFRAMEWORK || NET45_OR_GREATER
        ProgressChanged?.Invoke(this, value);
#endif
    }
}
#endif
