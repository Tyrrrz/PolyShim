#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net60_Stack
{
    extension<T>(Stack<T> stack)
    {
        // Note: Stack<T> does not expose a writable Capacity property prior to .NET 6.
        // This polyfill returns a value compatible with the .NET 6+ API contract but cannot
        // actually pre-allocate internal storage. The stack will resize on demand as entries
        // are added. This is a best-effort, API-compatible stub.
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.stack-1.ensurecapacity
        public int EnsureCapacity(int capacity) => Math.Max(capacity, stack.Count);
    }
}
#endif
