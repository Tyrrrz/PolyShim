#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
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
