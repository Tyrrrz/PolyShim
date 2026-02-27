#if (NETCOREAPP && !NET10_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Buffers;
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
                random.NextBytes(bytes.AsSpan(0, byteCount));

                var hex = lowercase
                    ? Convert.ToHexStringLower(bytes, 0, byteCount)
                    : Convert.ToHexString(bytes, 0, byteCount);

                return hex.Length == stringLength ? hex : hex.Substring(0, stringLength);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(bytes);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.random.getstring
        public string GetString(ReadOnlySpan<char> choices, int length)
        {
            var chars = ArrayPool<char>.Shared.Rent(length);
            try
            {
                for (var i = 0; i < length; i++)
                    chars[i] = choices[random.Next(choices.Length)];

                return new string(chars, 0, length);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(chars);
            }
        }
    }
}
#endif
