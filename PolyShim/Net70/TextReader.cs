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
            using var memoryOwner = MemoryPool<char>.Shared.Rent(4096);

            while (true)
            {
                var charsRead = await reader
                    .ReadAsync(memoryOwner.Memory, cancellationToken)
                    .ConfigureAwait(false);

                if (charsRead <= 0)
                    break;

                // Convert Memory slice to string for StringBuilder (allocates but necessary for older frameworks)
                result.Append(memoryOwner.Memory.Slice(0, charsRead).ToString());
            }

            return result.ToString();
        }
#endif
    }
}
#endif
