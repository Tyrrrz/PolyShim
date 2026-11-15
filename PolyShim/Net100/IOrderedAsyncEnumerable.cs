#if (NETCOREAPP && !NET10_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#if FEATURE_TASK && !FEATURE_ASYNC_LINQ
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq;

// https://learn.microsoft.com/dotnet/api/system.linq.iorderedasyncenumerable-1
internal interface IOrderedAsyncEnumerable<out T> : IAsyncEnumerable<T>
{
    // https://learn.microsoft.com/dotnet/api/system.linq.iorderedasyncenumerable-1.createorderedasyncenumerable?system-linq-iorderedasyncenumerable-1-createorderedasyncenumerable-1(system-func((-0-0))-system-collections-generic-icomparer((-0))-system-boolean)
    IOrderedAsyncEnumerable<T> CreateOrderedAsyncEnumerable<TKey>(
        Func<T, TKey> keySelector,
        IComparer<TKey>? comparer,
        bool descending
    );

    // https://learn.microsoft.com/dotnet/api/system.linq.iorderedasyncenumerable-1.createorderedasyncenumerable?system-linq-iorderedasyncenumerable-1-createorderedasyncenumerable-1(system-func((-0-system-threading-cancellationtoken-system-threading-tasks-valuetask((-1))))-system-collections-generic-icomparer((-1))-system-boolean)
    IOrderedAsyncEnumerable<T> CreateOrderedAsyncEnumerable<TKey>(
        Func<T, CancellationToken, ValueTask<TKey>> keySelector,
        IComparer<TKey>? comparer,
        bool descending
    );
}
#endif
#endif
