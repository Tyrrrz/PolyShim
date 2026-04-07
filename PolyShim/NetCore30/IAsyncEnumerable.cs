#if !FEATURE_ASYNCINTERFACES && FEATURE_VALUETASK && !(NETCOREAPP && !NETCOREAPP2_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
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
