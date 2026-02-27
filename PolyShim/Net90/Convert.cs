#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
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
    }
}
#endif
