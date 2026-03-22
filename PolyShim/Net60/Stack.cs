#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net60_Stack
{
    extension<T>(Stack<T> stack)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.stack-1.ensurecapacity
        public int EnsureCapacity(int capacity)
        {
            // Note: Stack<T> does not expose a writable Capacity property prior to .NET 6.
            // This polyfill returns the requested value but cannot actually pre-allocate
            // internal storage. The stack will resize on demand as entries are added. This is a
            // best-effort, API-compatible stub.
            return capacity;
        }
    }
}
#endif
