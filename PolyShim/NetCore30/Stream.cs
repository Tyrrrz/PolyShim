#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore30_Stream
{
    extension(Stream stream)
    {
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.stream.disposeasync
        public async Task DisposeAsync()
        {
#if FEATURE_ASYNCINTERFACES
            if (stream is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
                return;
            }
#endif

            try
            {
                await stream.FlushAsync();
            }
            finally
            {
                stream.Dispose();
            }
        }
#endif
    }
}
#endif
