#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
// HttpClient is not available on all target frameworks within this TFM range without a NuGet package reference
#if FEATURE_HTTPCLIENT
#nullable enable
#pragma warning disable CS0436

using System.Net.Http;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_HttpMethod
{
    extension(HttpMethod)
    {
        // https://learn.microsoft.com/dotnet/api/system.net.http.httpmethod.patch
        public static HttpMethod Patch => new("PATCH");
    }
}
#endif
#endif
