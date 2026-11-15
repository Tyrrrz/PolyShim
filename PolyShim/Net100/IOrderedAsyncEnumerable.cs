#if (NETCOREAPP && !NET10_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#if FEATURE_TASK
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;

namespace System.Linq;

// https://learn.microsoft.com/dotnet/api/system.linq.iorderedasyncenumerable-1
internal interface IOrderedAsyncEnumerable<out T> : IAsyncEnumerable<T>
{
    // https://learn.microsoft.com/dotnet/api/system.linq.iorderedasyncenumerable-1.createorderedenumerable
    IOrderedAsyncEnumerable<T> CreateOrderedEnumerable<TKey>(
        Func<T, TKey> keySelector,
        IComparer<TKey>? comparer,
        bool descending
    );
}
#endif
#endif
