#if !FEATURE_ASYNCINTERFACES
#if FEATURE_TASK
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Generic;

// https://learn.microsoft.com/dotnet/api/system.collections.generic.iasyncenumerable-1
internal interface IAsyncEnumerable<out T>
{
    IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default);
}

// https://learn.microsoft.com/dotnet/api/system.collections.generic.iasyncenumerator-1
internal interface IAsyncEnumerator<out T> : IAsyncDisposable
{
    T Current { get; }

    ValueTask<bool> MoveNextAsync();
}
#endif
#endif
