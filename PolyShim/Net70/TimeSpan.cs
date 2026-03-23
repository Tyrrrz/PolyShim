#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
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
