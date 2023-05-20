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
#if FEATURE_MEMORY
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.streamwriter.write#system-io-streamwriter-write(system-readonlyspan((system-char)))
    public static void Write(this StreamWriter writer, ReadOnlySpan<char> buffer)
    {
        var bufferArray = buffer.ToArray();
        writer.Write(bufferArray, 0, bufferArray.Length);
    }
#endif

#if FEATURE_TASK && FEATURE_MEMORY
    // https://learn.microsoft.com/en-us/dotnet/api/system.io.streamwriter.writeasync#system-io-streamwriter-writeasync(system-readonlymemory((system-char))-system-threading-cancellationtoken)
    public static async Task WriteAsync(
        this StreamWriter writer,
        Memory<char> buffer,
        CancellationToken cancellationToken = default)
    {
        var bufferArray = buffer.ToArray();

        cancellationToken.ThrowIfCancellationRequested();
        await writer.WriteAsync(bufferArray, 0, bufferArray.Length).ConfigureAwait(false);
    }
#endif
}
#endif