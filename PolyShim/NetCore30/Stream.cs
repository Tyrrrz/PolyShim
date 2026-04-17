#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore30_Stream
{
    extension(Stream stream)
    {
        // Task infrastructure is required for async method support.
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.stream.disposeasync
        public async ValueTask DisposeAsync()
        {
            // IAsyncDisposable is provided via the Microsoft.Bcl.AsyncInterfaces NuGet package.
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
