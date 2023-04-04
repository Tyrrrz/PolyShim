#if (NETCOREAPP1_0_OR_GREATER && !NET5_0_OR_GREATER) || (NET20_OR_GREATER) || (NETSTANDARD1_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

[ExcludeFromCodeCoverage]
internal static class _0F5BD49375DC4FC586A551C526A627DC
{
#if FEATURE_TASK && FEATURE_HTTP
    // Documentation missing on docs.microsoft.com for this signature
    public static async Task<Stream> ReadAsStreamAsync(
        this HttpContent httpContent,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
    }

    // Documentation missing on docs.microsoft.com for this signature
    public static async Task<byte[]> ReadAsByteArrayAsync(
        this HttpContent httpContent,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await httpContent.ReadAsByteArrayAsync().ConfigureAwait(false);
    }

    // Documentation missing on docs.microsoft.com for this signature
    public static async Task<string> ReadAsStringAsync(
        this HttpContent httpContent,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await httpContent.ReadAsStringAsync().ConfigureAwait(false);
    }
#endif
}
#endif