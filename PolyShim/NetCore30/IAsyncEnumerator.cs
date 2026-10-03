#if !FEATURE_ASYNCINTERFACES
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System.Threading.Tasks;

namespace System.Collections.Generic;

// https://learn.microsoft.com/dotnet/api/system.collections.generic.iasyncenumerator-1
internal interface IAsyncEnumerator<out T> : IAsyncDisposable
{
    T Current { get; }

    ValueTask<bool> MoveNextAsync();
}
#endif
#endif
