#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Threading;

internal partial class Lock
{
#if (NETCOREAPP) || (NETFRAMEWORK && NET45_OR_GREATER) || (NETSTANDARD)
    public bool IsHeldByCurrentThread => Monitor.IsEntered(this);
#endif

    public void Enter() => Monitor.Enter(this);

    public bool TryEnter() => Monitor.TryEnter(this);

    public bool TryEnter(TimeSpan timeout) => Monitor.TryEnter(this, timeout);

    public bool TryEnter(int millisecondsTimeout) => TryEnter(TimeSpan.FromMilliseconds(millisecondsTimeout));

    public void Exit() => Monitor.Exit(this);

    public Scope EnterScope()
    {
        Enter();
        return new Scope(this);
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
