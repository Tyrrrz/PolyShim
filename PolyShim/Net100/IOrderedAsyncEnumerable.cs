#if (NETCOREAPP && !NET10_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// Task and ValueTask are not available on all target frameworks within this TFM range without a NuGet package reference
#if FEATURE_TASK && FEATURE_VALUETASK
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
