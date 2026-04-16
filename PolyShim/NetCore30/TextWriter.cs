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
internal static class MemberPolyfills_NetCore30_TextWriter
{
    extension(TextWriter writer)
    {
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.textwriter.disposeasync
        public async ValueTask DisposeAsync()
        {
#if FEATURE_ASYNCINTERFACES
            if (writer is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
                return;
            }
#endif

            try
            {
                await writer.FlushAsync();
            }
            finally
            {
                writer.Dispose();
            }
        }
#endif
    }
}
#endif
