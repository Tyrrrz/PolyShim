#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_DateTimeOffset
{
    extension(DateTimeOffset)
    {
        // https://learn.microsoft.com/dotnet/api/system.datetimeoffset.unixepoch
        public static DateTimeOffset UnixEpoch => new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
    }
}
#endif
