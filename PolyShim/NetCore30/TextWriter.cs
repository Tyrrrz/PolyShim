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

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore30_TextWriter
{
    extension(TextWriter writer)
    {
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.textwriter.disposeasync
        public async Task DisposeAsync()
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
