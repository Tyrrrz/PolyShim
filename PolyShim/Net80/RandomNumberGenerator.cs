#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net80_RandomNumberGenerator
{
    extension(RandomNumberGenerator)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getitems#system-security-cryptography-randomnumbergenerator-getitems-1(system-readonlyspan((-0))-system-span((-0)))
        public static void GetItems<T>(ReadOnlySpan<T> choices, Span<T> destination)
        {
            for (var i = 0; i < destination.Length; i++)
            {
                var index = RandomNumberGenerator.GetInt32(choices.Length);
                destination[i] = choices[index];
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getitems#system-security-cryptography-randomnumbergenerator-getitems-1(system-readonlyspan((-0))-system-int32)
        public static T[] GetItems<T>(ReadOnlySpan<T> choices, int length)
        {
            var result = new T[length];
            RandomNumberGenerator.GetItems(choices, result.AsSpan());
            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.shuffle#system-security-cryptography-randomnumbergenerator-shuffle-1(system-span((-0)))
        public static void Shuffle<T>(Span<T> items)
        {
            for (var i = items.Length - 1; i > 0; i--)
            {
                var j = RandomNumberGenerator.GetInt32(i + 1);
                (items[i], items[j]) = (items[j], items[i]);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.gethexstring#system-security-cryptography-randomnumbergenerator-gethexstring(system-int32-system-boolean)
        public static string GetHexString(int stringLength, bool lowercase = false)
        {
            var byteCount = (stringLength + 1) / 2;
            var bytes = ArrayPool<byte>.Shared.Rent(byteCount);
            try
            {
                RandomNumberGenerator.Fill(bytes.AsSpan(0, byteCount));

                var hex = lowercase
                    ? Convert.ToHexStringLower(bytes, 0, byteCount)
                    : Convert.ToHexString(bytes, 0, byteCount);

                return hex.Length == stringLength ? hex : hex.Substring(0, stringLength);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(bytes, clearArray: true);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getstring
        public static string GetString(ReadOnlySpan<char> choices, int length)
        {
            var chars = ArrayPool<char>.Shared.Rent(length);
            try
            {
                for (var i = 0; i < length; i++)
                    chars[i] = choices[RandomNumberGenerator.GetInt32(choices.Length)];

                return new string(chars, 0, length);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(chars, clearArray: true);
            }
        }
    }
}
#endif
