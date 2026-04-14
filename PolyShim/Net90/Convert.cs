#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net90_Convert
{
    extension(Convert)
    {
        // https://learn.microsoft.com/dotnet/api/system.convert.tohexstringlower#system-convert-tohexstringlower(system-byte()-system-int32-system-int32)
        public static string ToHexStringLower(byte[] value, int startIndex, int length) =>
            Convert.ToHexString(value, startIndex, length).ToLowerInvariant();

        // https://learn.microsoft.com/dotnet/api/system.convert.tohexstringlower#system-convert-tohexstringlower(system-byte())
        public static string ToHexStringLower(byte[] value) =>
            ToHexStringLower(value, 0, value.Length);

        // https://learn.microsoft.com/dotnet/api/system.convert.tohexstringlower#system-convert-tohexstringlower(system-readonlyspan((system-byte)))
        public static string ToHexStringLower(ReadOnlySpan<byte> bytes) =>
            Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
#endif
