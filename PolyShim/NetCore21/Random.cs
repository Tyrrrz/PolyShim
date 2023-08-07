#if FEATURE_MEMORY
#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;

internal static partial class PolyfillExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.random.nextbytes#system-random-nextbytes(system-span((system-byte)))
    public static void NextBytes(this Random random, Span<byte> buffer)
    {
        var bufferArray = buffer.ToArray();
        random.NextBytes(bufferArray);
        bufferArray.CopyTo(buffer);
    }
}
#endif
#endif