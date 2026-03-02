#if FEATURE_HTTPCLIENT
#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_HttpClient
{
#if FEATURE_TASK
    extension(HttpClient httpClient)
    {
        // https://learn.microsoft.com/dotnet/api/system.net.http.httpclient.getstreamasync#system-net-http-httpclient-getstreamasync(system-string-system-threading-cancellationtoken)
        public async Task<Stream> GetStreamAsync(string requestUri,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Must not be disposed for the stream to be usable
                var response = await httpClient.GetAsync(
                    requestUri,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken
                ).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            }
            // Older versions of HttpClient methods don't propagate the cancellation token inside the exception
            catch (OperationCanceledException ex) when (
                ex.CancellationToken != cancellationToken &&
                cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException(ex.Message, ex.InnerException, cancellationToken);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.net.http.httpclient.getstreamasync#system-net-http-httpclient-getstreamasync(system-uri-system-threading-cancellationtoken)
        public async Task<Stream> GetStreamAsync(Uri requestUri,
            CancellationToken cancellationToken = default) =>
            await httpClient.GetStreamAsync(requestUri.ToString(), cancellationToken).ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.net.http.httpclient.getbytearrayasync#system-net-http-httpclient-getbytearrayasync(system-string-system-threading-cancellationtoken)
        public async Task<byte[]> GetByteArrayAsync(string requestUri,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var response = await httpClient.GetAsync(
                    requestUri,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken
                ).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
            }
            // Older versions of HttpClient methods don't propagate the cancellation token inside the exception
            catch (OperationCanceledException ex) when (
                ex.CancellationToken != cancellationToken &&
                cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException(ex.Message, ex.InnerException, cancellationToken);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.net.http.httpclient.getbytearrayasync#system-net-http-httpclient-getbytearrayasync(system-uri-system-threading-cancellationtoken)
        public async Task<byte[]> GetByteArrayAsync(Uri requestUri,
            CancellationToken cancellationToken = default) =>
            await httpClient.GetByteArrayAsync(requestUri.ToString(), cancellationToken).ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.net.http.httpclient.getstringasync#system-net-http-httpclient-getstringasync(system-string-system-threading-cancellationtoken)
        public async Task<string> GetStringAsync(string requestUri,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var response = await httpClient.GetAsync(
                    requestUri,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken
                ).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            }
            // Older versions of HttpClient methods don't propagate the cancellation token inside the exception
            catch (OperationCanceledException ex) when (
                ex.CancellationToken != cancellationToken &&
                cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException(ex.Message, ex.InnerException, cancellationToken);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.net.http.httpclient.getstringasync#system-net-http-httpclient-getstringasync(system-uri-system-threading-cancellationtoken)
        public async Task<string> GetStringAsync(Uri requestUri,
            CancellationToken cancellationToken = default) =>
            await httpClient.GetStringAsync(requestUri.ToString(), cancellationToken).ConfigureAwait(false);
    }
#endif
}
#endif
#endif
