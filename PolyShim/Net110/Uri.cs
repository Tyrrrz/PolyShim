#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net110_Uri
{
    extension(Uri)
    {
        // https://learn.microsoft.com/dotnet/api/system.uri.urischemedata
        public static string UriSchemeData => "data";
    }
}
#endif
