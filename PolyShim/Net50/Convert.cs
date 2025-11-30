#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;

internal static partial class PolyfillExtensions
{
    extension(Convert)
    {
        // https://learn.microsoft.com/dotnet/api/system.convert.fromhexstring#system-convert-fromhexstring(system-string)
        public static byte[] FromHexString(string s)
        {
            static int GetHexValue(char c)
            {
                if (c is >= '0' and <= '9')
                    return c - '0';
                if (c is >= 'A' and <= 'F')
                    return c - 'A' + 10;
                if (c is >= 'a' and <= 'f')
                    return c - 'a' + 10;

                throw new FormatException($"Invalid hex character '{c}'.");
            }

            if (s.Length % 2 != 0)
                throw new FormatException("The hex string must have an even length.");

            var byteCount = s.Length / 2;
            var bytes = new byte[byteCount];

            for (var i = 0; i < byteCount; i++)
            {
                var highNibble = s[i * 2];
                var lowNibble = s[i * 2 + 1];

                bytes[i] = (byte)((GetHexValue(highNibble) << 4) + GetHexValue(lowNibble));
            }

            return bytes;
        }

        // https://learn.microsoft.com/dotnet/api/system.convert.tohexstring#system-convert-tohexstring(system-byte()-system-int32-system-int32)
        public static string ToHexString(byte[] value, int startIndex, int length)
        {
            var c = new char[length * 2];

            for (var i = 0; i < length; i++)
            {
                var b = value[startIndex + i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = value[startIndex + i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }

            return new string(c);
        }

        // https://learn.microsoft.com/dotnet/api/system.convert.tohexstring#system-convert-tohexstring(system-byte())
        public static string ToHexString(byte[] value) => ToHexString(value, 0, value.Length);
    }
}
#endif
