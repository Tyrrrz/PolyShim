#if (NETCOREAPP && !NET10_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Buffers;
using System.Text;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net100_Random
{
    extension(Random random)
    {
        // https://learn.microsoft.com/dotnet/api/system.random.gethexstring#system-random-gethexstring(system-int32-system-boolean)
        public string GetHexString(int stringLength, bool lowercase = false)
        {
            var byteCount = (stringLength + 1) / 2;
            var bytes = ArrayPool<byte>.Shared.Rent(byteCount);
            try
            {
                random.NextBytes(bytes);

                var hex = lowercase ? Convert.ToHexStringLower(bytes) : Convert.ToHexString(bytes);
                return hex.Substring(0, stringLength);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(bytes);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.random.getstring
        public string GetString(ReadOnlySpan<char> choices, int length)
        {
            var buffer = new StringBuilder(length);

            for (var i = 0; i < length; i++)
            {
                var index = random.Next(choices.Length);
                buffer.Append(choices[index]);
            }

            return buffer.ToString();
        }
    }
}
#endif
