#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
// HttpClient is not available on all target frameworks within this TFM range without a NuGet package reference
#if FEATURE_HTTPCLIENT
#nullable enable
#pragma warning disable CS0436

using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_HttpContent
{
    // Task infrastructure is required for async method support
#if FEATURE_TASK
    extension(HttpContent httpContent)
    {
        // https://learn.microsoft.com/dotnet/api/system.net.http.httpcontent.readasstreamasync#system-net-http-httpcontent-readasstreamasync(system-threading-cancellationtoken)
        public async Task<Stream> ReadAsStreamAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.net.http.httpcontent.readasbytearrayasync#system-net-http-httpcontent-readasbytearrayasync(system-threading-cancellationtoken)
        public async Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await httpContent.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.net.http.httpcontent.readasstringasync#system-net-http-httpcontent-readasstringasync(system-threading-cancellationtoken)
        public async Task<string> ReadAsStringAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await httpContent.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
#endif
}
#endif
#endif
