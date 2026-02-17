#if (NETCOREAPP && !NET8_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_Net80_RandomNumberGenerator
{
    extension(RandomNumberGenerator)
    {
        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getitems#system-security-cryptography-randomnumbergenerator-getitems-1(-0()-system-int32)
        public static T[] GetItems<T>(T[] choices, int length)
        {
            if (choices is null)
                throw new ArgumentNullException(nameof(choices));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            var result = new T[length];
            for (var i = 0; i < length; i++)
            {
                var index = RandomNumberGenerator.GetInt32(choices.Length);
                result[i] = choices[index];
            }
            return result;
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.shuffle#system-security-cryptography-randomnumbergenerator-shuffle-1(-0())
        public static void Shuffle<T>(T[] items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            for (var i = items.Length - 1; i > 0; i--)
            {
                var j = RandomNumberGenerator.GetInt32(i + 1);
                (items[i], items[j]) = (items[j], items[i]);
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.gethexstring#system-security-cryptography-randomnumbergenerator-gethexstring(system-int32-system-boolean)
        public static string GetHexString(int stringLength, bool lowercase = false)
        {
            if (stringLength < 0)
                throw new ArgumentOutOfRangeException(nameof(stringLength));

            var bytes = new byte[(stringLength + 1) / 2];
            RandomNumberGenerator.Fill(bytes);

            var hex = lowercase ? Convert.ToHexStringLower(bytes) : Convert.ToHexString(bytes);
            return hex.Substring(0, stringLength);
        }

        // https://learn.microsoft.com/dotnet/api/system.security.cryptography.randomnumbergenerator.getstring
        public static string GetString(ReadOnlySpan<char> choices, int length)
        {
            if (choices.Length == 0)
                throw new ArgumentException("choices must not be empty", nameof(choices));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            var buffer = new StringBuilder(length);

            for (var i = 0; i < length; i++)
            {
                var index = RandomNumberGenerator.GetInt32(choices.Length);
                buffer.Append(choices[index]);
            }

            return buffer.ToString();
        }
    }
}
#endif
