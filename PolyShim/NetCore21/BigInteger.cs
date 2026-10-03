#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
// BigInteger is only available on .NET Framework 4.0+ and .NET Standard 1.1+
#if (!NETFRAMEWORK || NET40_OR_GREATER) && (!NETSTANDARD || NETSTANDARD1_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_BigInteger
{
    extension(BigInteger value)
    {
        // https://learn.microsoft.com/dotnet/api/system.numerics.biginteger.getbytecount
        public int GetByteCount(bool isUnsigned = false) =>
            value.ToByteArray(isUnsigned, isBigEndian: false).Length;

        // https://learn.microsoft.com/dotnet/api/system.numerics.biginteger.tobytearray#system-numerics-biginteger-tobytearray(system-boolean-system-boolean)
        public byte[] ToByteArray(bool isUnsigned = false, bool isBigEndian = false)
        {
            if (isUnsigned && value.Sign < 0)
                throw new OverflowException(
                    "Negative values do not have an unsigned representation."
                );

            var bytes = value.ToByteArray();

            // The signed little-endian representation may contain an extra zero byte in the
            // most significant position, used solely to disambiguate the sign. That byte is
            // redundant for an unsigned representation, so it can be safely dropped.
            if (isUnsigned && bytes.Length > 1 && bytes[bytes.Length - 1] == 0)
            {
                var trimmedBytes = new byte[bytes.Length - 1];
                Array.Copy(bytes, trimmedBytes, trimmedBytes.Length);
                bytes = trimmedBytes;
            }

            if (isBigEndian)
                Array.Reverse(bytes);

            return bytes;
        }

        // https://learn.microsoft.com/dotnet/api/system.numerics.biginteger.trywritebytes
        public bool TryWriteBytes(
            Span<byte> destination,
            out int bytesWritten,
            bool isUnsigned = false,
            bool isBigEndian = false
        )
        {
            var bytes = value.ToByteArray(isUnsigned, isBigEndian);
            if (bytes.Length > destination.Length)
            {
                bytesWritten = 0;
                return false;
            }

            bytes.CopyTo(destination);
            bytesWritten = bytes.Length;

            return true;
        }
    }
}
#endif
#endif
