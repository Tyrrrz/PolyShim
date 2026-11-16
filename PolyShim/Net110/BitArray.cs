#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net110_BitArray
{
    extension(BitArray bitArray)
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.bitarray.popcount
        public int PopCount()
        {
            ArgumentNullException.ThrowIfNull(bitArray);

            var count = 0;
            for (var i = 0; i < bitArray.Count; i++)
            {
                if (bitArray[i])
                    count++;
            }

            return count;
        }
    }
}
#endif
