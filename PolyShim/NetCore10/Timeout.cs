#if (NETFRAMEWORK && !NET45_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_Timeout
{
    extension(Timeout)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.timeout.infinitetimespan
        public static TimeSpan InfiniteTimeSpan => TimeSpan.FromMilliseconds(Timeout.Infinite);
    }
}
#endif
