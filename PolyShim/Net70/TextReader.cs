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
            using var buffer = MemoryPool<char>.Shared.Rent(4096);

            while (true)
            {
                var charsRead = await reader
                    .ReadAsync(buffer.Memory, cancellationToken)
                    .ConfigureAwait(false);

                if (charsRead <= 0)
                    break;

                // Append char by char to avoid string allocation
                for (var i = 0; i < charsRead; i++)
                {
                    result.Append(buffer.Memory.Span[i]);
                }
            }

            return result.ToString();
        }
#endif
    }
}
#endif
