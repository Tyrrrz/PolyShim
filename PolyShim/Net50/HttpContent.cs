#if FEATURE_HTTPCLIENT
#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

internal static partial class PolyfillExtensions
{
#if FEATURE_TASK
    extension(HttpContent httpContent)
    {
        // Documentation missing on docs.microsoft.com for this signature
        public async Task<Stream> ReadAsStreamAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
        }

        // Documentation missing on docs.microsoft.com for this signature
        public async Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await httpContent.ReadAsByteArrayAsync().ConfigureAwait(false);
        }

        // Documentation missing on docs.microsoft.com for this signature
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
