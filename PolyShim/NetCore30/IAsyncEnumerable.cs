#if !FEATURE_ASYNCINTERFACES
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System.Threading;

namespace System.Collections.Generic;

// https://learn.microsoft.com/dotnet/api/system.collections.generic.iasyncenumerable-1
internal interface IAsyncEnumerable<out T>
{
    IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default);
}
#endif
#endif
