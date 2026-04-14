#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
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
