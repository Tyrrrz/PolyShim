#if (NETCOREAPP && !NET10_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net100_TimeSpan
{
    extension(TimeSpan)
    {
        // https://learn.microsoft.com/dotnet/api/system.timespan.frommilliseconds#system-timespan-frommilliseconds(system-int64)
        public static TimeSpan FromMilliseconds(long milliseconds) =>
            TimeSpan.FromMilliseconds(milliseconds, 0);
    }
}
#endif
