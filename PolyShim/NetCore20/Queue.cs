#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_Queue
{
    extension<T>(Queue<T> queue)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.queue-1.trydequeue
        public bool TryDequeue(out T? result)
        {
            if (queue.Count > 0)
            {
                result = queue.Dequeue();
                return true;
            }

            result = default;
            return false;
        }

        // https://learn.microsoft.com/dotnet/api/system.collections.generic.queue-1.trypeek
        public bool TryPeek(out T? result)
        {
            if (queue.Count > 0)
            {
                result = queue.Peek();
                return true;
            }

            result = default;
            return false;
        }
    }
}
#endif
