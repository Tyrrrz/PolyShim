#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
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
internal static class MemberPolyfills_NetCore21_DateTime
{
    extension(DateTime)
    {
        // https://learn.microsoft.com/dotnet/api/system.datetime.unixepoch
        public static DateTime UnixEpoch => new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
#endif
