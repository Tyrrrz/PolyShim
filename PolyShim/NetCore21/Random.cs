#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_Random
{
    extension(Random random)
    {
        // https://learn.microsoft.com/dotnet/api/system.random.nextbytes#system-random-nextbytes(system-span((system-byte)))
        public void NextBytes(Span<byte> buffer)
        {
            var bufferArray = new byte[buffer.Length];
            random.NextBytes(bufferArray);
            bufferArray.CopyTo(buffer);
        }
    }
}
#endif
