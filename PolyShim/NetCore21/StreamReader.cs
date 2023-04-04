#if (NETCOREAPP1_0_OR_GREATER && !NETCOREAPP2_1_OR_GREATER) || (NET20_OR_GREATER) || (NETSTANDARD1_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

[ExcludeFromCodeCoverage]
internal static class _F21D610E49CC4FA0A62C2D9D0A1CBFC8
{
    // Signature-compatible replacement for Read(Span<char>)
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader.read#system-io-streamreader-read(system-span((system-char)))
    public static int Read(this StreamReader reader, char[] buffer) =>
        reader.Read(buffer, 0, buffer.Length);

#if FEATURE_TASK
    // Signature-compatible replacement for ReadAsync(Memory<char>)
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader.readasync#system-io-streamreader-readasync(system-memory((system-char))-system-threading-cancellationtoken)
    public static async Task<int> ReadAsync(
        this StreamReader reader,
        char[] buffer,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await reader.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
    }
#endif

#if FEATURE_MEMORY
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader.read#system-io-streamreader-read(system-span((system-char)))
    public static int Read(this StreamReader reader, Span<char> buffer)
    {
        var bufferArray = buffer.ToArray();
        var result = reader.Read(bufferArray, 0, bufferArray.Length);

        bufferArray.CopyTo(buffer);

        return result;
    }
#endif

#if FEATURE_TASK && FEATURE_MEMORY
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader.readasync#system-io-streamreader-readasync(system-memory((system-char))-system-threading-cancellationtoken)
    public static async Task<int> ReadAsync(
        this StreamReader reader,
        Memory<char> buffer,
        CancellationToken cancellationToken = default)
    {
        var bufferArray = buffer.ToArray();

        cancellationToken.ThrowIfCancellationRequested();
        var result = await reader.ReadAsync(bufferArray, 0, bufferArray.Length).ConfigureAwait(false);

        bufferArray.CopyTo(buffer);

        return result;
    }
#endif
}
#endif