#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore21_Random
{
    extension(Random random)
    {
        // https://learn.microsoft.com/dotnet/api/system.random.nextbytes#system-random-nextbytes(system-span((system-byte)))
        public void NextBytes(Span<byte> buffer)
        {
            if (buffer.IsEmpty)
                return;

            var bufferArray = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try
            {
                random.NextBytes(bufferArray);
                bufferArray.AsSpan(0, buffer.Length).CopyTo(buffer);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(bufferArray);
            }
        }
    }
}
#endif
