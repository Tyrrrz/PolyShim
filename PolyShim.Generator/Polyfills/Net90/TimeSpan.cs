#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
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
internal static class MemberPolyfills_Net90_TimeSpan
{
    extension(TimeSpan)
    {
        // https://learn.microsoft.com/dotnet/api/system.timespan.frommilliseconds#system-timespan-frommilliseconds(system-int64-system-int64)
        public static TimeSpan FromMilliseconds(long milliseconds, long microseconds) =>
            TimeSpan.FromMilliseconds(milliseconds + microseconds / 1_000.0);
    }
}
#endif
