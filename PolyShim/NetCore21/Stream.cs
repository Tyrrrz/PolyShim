#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

internal static partial class PolyfillExtensions
{
    // Signature-compatible replacement for Read(Span<byte>)
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.read#system-io-stream-read(system-span((system-byte)))
    public static int Read(this Stream stream, byte[] buffer) =>
        stream.Read(buffer, 0, buffer.Length);

    // Signature-compatible replacement for Write(ReadOnlySpan<byte>)
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.write#system-io-stream-write(system-readonlyspan((system-byte)))
    public static void Write(this Stream stream, byte[] buffer) =>
        stream.Write(buffer, 0, buffer.Length);

#if FEATURE_TASK
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.copytoasync#system-io-stream-copytoasync(system-io-stream-system-threading-cancellationtoken)
    public static async Task CopyToAsync(
        this Stream stream,
        Stream destination,
        CancellationToken cancellationToken = default) =>
        await stream.CopyToAsync(destination, 81920, cancellationToken).ConfigureAwait(false);

    // Signature-compatible replacement for ReadAsync(Memory<byte>, ...)
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.readasync#system-io-stream-readasync(system-memory((system-byte))-system-threading-cancellationtoken)
    public static async Task<int> ReadAsync(
        this Stream stream,
        byte[] buffer,
        CancellationToken cancellationToken = default) =>
        await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);

    // Signature-compatible replacement for WriteAsync(ReadOnlyMemory<byte>, ...)
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.writeasync#system-io-stream-writeasync(system-readonlymemory((system-byte))-system-threading-cancellationtoken)
    public static async Task WriteAsync(
        this Stream stream,
        byte[] buffer,
        CancellationToken cancellationToken = default) =>
        await stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
#endif

#if FEATURE_MEMORY
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.read#system-io-stream-read(system-span((system-byte)))
    public static int Read(this Stream stream, Span<byte> buffer)
    {
        var bufferArray = buffer.ToArray();
        var result = stream.Read(bufferArray, 0, bufferArray.Length);
        bufferArray.CopyTo(buffer);

        return result;
    }

    // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.write#system-io-stream-write(system-readonlyspan((system-byte)))
    public static void Write(this Stream stream, ReadOnlySpan<byte> buffer)
    {
        var bufferArray = buffer.ToArray();
        stream.Write(bufferArray, 0, bufferArray.Length);
    }
#endif

#if FEATURE_TASK && FEATURE_MEMORY
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.readasync#system-io-stream-readasync(system-memory((system-byte))-system-threading-cancellationtoken)
    public static async Task<int> ReadAsync(
        this Stream stream,
        Memory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        var bufferArray = buffer.ToArray();
        var result = await stream.ReadAsync(bufferArray, 0, bufferArray.Length, cancellationToken)
            .ConfigureAwait(false);

        bufferArray.CopyTo(buffer);

        return result;
    }

    // Task instead of ValueTask for maximum compatibility
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.stream.writeasync#system-io-stream-writeasync(system-readonlymemory((system-byte))-system-threading-cancellationtoken)
    public static async Task WriteAsync(
        this Stream stream,
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        var bufferArray = buffer.ToArray();
        await stream.WriteAsync(bufferArray, 0, bufferArray.Length, cancellationToken)
            .ConfigureAwait(false);
    }
#endif
}
#endif