#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_TimeSpan
{
    extension(TimeSpan)
    {
        // https://learn.microsoft.com/dotnet/api/system.timespan.frommicroseconds
        public static TimeSpan FromMicroseconds(double microseconds) =>
            TimeSpan.FromTicks((long)(microseconds * 10));
    }
}
#endif
