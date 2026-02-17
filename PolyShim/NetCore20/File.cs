#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore20_File
{
    // No file I/O on .NET Standard prior to 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
    extension(File)
    {
#if FEATURE_TASK
        // https://learn.microsoft.com/dotnet/api/system.io.file.appendalllinesasync#system-io-file-appendalllinesasync(system-string-system-collections-generic-ienumerable((system-string))-system-threading-cancellationtoken)
        public static async Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true);
            using var writer = new StreamWriter(stream);

            foreach (var line in contents)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await writer.WriteLineAsync(line).ConfigureAwait(false);
            }

            await writer.FlushAsync().ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.appendalllinesasync#system-io-file-appendalllinesasync(system-string-system-collections-generic-ienumerable((system-string))-system-text-encoding-system-threading-cancellationtoken)
        public static async Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true);
            using var writer = new StreamWriter(stream, encoding);

            foreach (var line in contents)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await writer.WriteLineAsync(line).ConfigureAwait(false);
            }

            await writer.FlushAsync().ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.appendalltextasync#system-io-file-appendalltextasync(system-string-system-string-system-text-encoding-system-threading-cancellationtoken)
        public static async Task AppendAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true);
            using var writer = new StreamWriter(stream, encoding);

            cancellationToken.ThrowIfCancellationRequested();
            await writer.WriteAsync(contents).ConfigureAwait(false);
            await writer.FlushAsync().ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.appendalltextasync#system-io-file-appendalltextasync(system-string-system-string-system-threading-cancellationtoken)
        public static async Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true);
            using var writer = new StreamWriter(stream);

            cancellationToken.ThrowIfCancellationRequested();
            await writer.WriteAsync(contents).ConfigureAwait(false);
            await writer.FlushAsync().ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.readallbytesasync
        public static async Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            using var buffer = new MemoryStream();

            await stream.CopyToAsync(buffer, 81920, cancellationToken).ConfigureAwait(false);

            return buffer.ToArray();
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.readalllinesasync#system-io-file-readalllinesasync(system-string-system-threading-cancellationtoken)
        public static async Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default)
        {
            var lines = new List<string>();

            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var line = await reader.ReadLineAsync().ConfigureAwait(false);
                if (line is not null)
                {
                    lines.Add(line);
                }
            }

            return lines.ToArray();
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.readalllinesasync#system-io-file-readalllinesasync(system-string-system-text-encoding-system-threading-cancellationtoken)
        public static async Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
        {
            var lines = new List<string>();

            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            using var reader = new StreamReader(stream, encoding);

            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var line = await reader.ReadLineAsync().ConfigureAwait(false);
                if (line is not null)
                {
                    lines.Add(line);
                }
            }

            return lines.ToArray();
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.readalltextasync#system-io-file-readalltextasync(system-string-system-threading-cancellationtoken)
        public static async Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            using var reader = new StreamReader(stream);

            var content = new StringBuilder();
            var buffer = ArrayPool<char>.Shared.Rent(4096);
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                int charsRead;
                while ((charsRead = await reader.ReadAsync(buffer, 0, 4096).ConfigureAwait(false)) > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    content.Append(buffer, 0, charsRead);
                }

                return content.ToString();
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buffer);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.readalltextasync#system-io-file-readalltextasync(system-string-system-text-encoding-system-threading-cancellationtoken)
        public static async Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            using var reader = new StreamReader(stream, encoding);

            var content = new StringBuilder();
            var buffer = ArrayPool<char>.Shared.Rent(4096);
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                int charsRead;
                while ((charsRead = await reader.ReadAsync(buffer, 0, 4096).ConfigureAwait(false)) > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    content.Append(buffer, 0, charsRead);
                }

                return content.ToString();
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buffer);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.writeallbytesasync#system-io-file-writeallbytesasync(system-string-system-byte()-system-threading-cancellationtoken)
        public static async Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
            await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.writealllinesasync#system-io-file-writealllinesasync(system-string-system-collections-generic-ienumerable((system-string))-system-threading-cancellationtoken)
        public static async Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            using var writer = new StreamWriter(stream);

            foreach (var line in contents)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await writer.WriteLineAsync(line).ConfigureAwait(false);
            }

            await writer.FlushAsync().ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.writealllinesasync#system-io-file-writealllinesasync(system-string-system-collections-generic-ienumerable((system-string))-system-text-encoding-system-threading-cancellationtoken)
        public static async Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            using var writer = new StreamWriter(stream, encoding);

            foreach (var line in contents)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await writer.WriteLineAsync(line).ConfigureAwait(false);
            }

            await writer.FlushAsync().ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.writealltextasync#system-io-file-writealltextasync(system-string-system-string-system-threading-cancellationtoken)
        public static async Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            using var writer = new StreamWriter(stream);

            cancellationToken.ThrowIfCancellationRequested();
            await writer.WriteAsync(contents).ConfigureAwait(false);
            await writer.FlushAsync().ConfigureAwait(false);
        }

        // https://learn.microsoft.com/dotnet/api/system.io.file.writealltextasync#system-io-file-writealltextasync(system-string-system-string-system-text-encoding-system-threading-cancellationtoken)
        public static async Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            using var writer = new StreamWriter(stream, encoding);

            cancellationToken.ThrowIfCancellationRequested();
            await writer.WriteAsync(contents).ConfigureAwait(false);
            await writer.FlushAsync().ConfigureAwait(false);
        }
#endif
    }
#endif
}
#endif
