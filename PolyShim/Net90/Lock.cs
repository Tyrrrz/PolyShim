#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS9216
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Threading;

// https://learn.microsoft.com/dotnet/api/system.threading.lock
[ExcludeFromCodeCoverage]
internal partial class Lock
{
#if (NETCOREAPP) || (NETFRAMEWORK && NET45_OR_GREATER) || (NETSTANDARD)
    public bool IsHeldByCurrentThread => Monitor.IsEntered(this);
#endif

    public void Enter() => Monitor.Enter(this);

    public bool TryEnter() => Monitor.TryEnter(this);

    public bool TryEnter(TimeSpan timeout) => Monitor.TryEnter(this, timeout);

    public bool TryEnter(int millisecondsTimeout) =>
        TryEnter(TimeSpan.FromMilliseconds(millisecondsTimeout));

    public void Exit() => Monitor.Exit(this);

    public Scope EnterScope()
    {
#if NETFRAMEWORK && !NET40_OR_GREATER
        // Older versions of the framework don't have the overload of Monitor.Enter(...) that accepts a ref bool
        Monitor.Enter(this);
        return new Scope(this);
#else
        var acquiredLock = false;
        try
        {
            Monitor.Enter(this, ref acquiredLock);
            return new Scope(this);
        }
        // Ensure that the lock is released if the owning thread is aborted.
        // Implementation reference:
        // https://github.com/MarkCiliaVincenti/Backport.System.Threading.Lock/blob/c28041f1e22e561d5cde040704abeeb8d9a18649/Backport.System.Threading.Lock/PreNet5Lock.cs#L112-L125
        // MIT License, Mark Cilia Vincenti
        // https://github.com/Tyrrrz/PolyShim/pull/10#issuecomment-2381456516
        catch (ThreadAbortException)
        {
            if (acquiredLock)
                Monitor.Exit(this);

            throw;
        }
#endif
    }
}

internal partial class Lock
{
    public readonly ref struct Scope(Lock owner)
    {
        public void Dispose() => owner.Exit();
    }
}
#endif
