#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net70_TextReader
{
    extension(TextReader reader)
    {
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.textreader.readlineasync#system-io-textreader-readlineasync(system-threading-cancellationtoken)
        public Task<string?> ReadLineAsync(CancellationToken cancellationToken)
        {
            // Impossible to polyfill this properly as it requires to track the buffer's state
            cancellationToken.ThrowIfCancellationRequested();
            return reader.ReadLineAsync();
        }

        // https://learn.microsoft.com/dotnet/api/system.io.textreader.readtoendasync#system-io-textreader-readtoendasync(system-threading-cancellationtoken)
        public async Task<string> ReadToEndAsync(CancellationToken cancellationToken)
        {
            var result = new StringBuilder();
            var buffer = ArrayPool<char>.Shared.Rent(4096);
            try
            {
                while (true)
                {
                    // Slice to read exactly 4096 chars, not the full pooled buffer size
                    var charsRead = await reader
                        .ReadAsync(buffer.AsMemory(0, 4096), cancellationToken)
                        .ConfigureAwait(false);

                    if (charsRead <= 0)
                        break;

                    result.Append(buffer, 0, charsRead);
                }

                return result.ToString();
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buffer);
            }
        }
#endif
    }
}
#endif
