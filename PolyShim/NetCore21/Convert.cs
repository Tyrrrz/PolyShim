#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore21_Convert
{
    extension(Convert)
    {
        // https://learn.microsoft.com/dotnet/api/system.convert.tobase64string?system-convert-tobase64string(system-readonlyspan((system-byte))-system-base64formattingoptions)
        public static string ToBase64String(
            ReadOnlySpan<byte> bytes,
            Base64FormattingOptions options = Base64FormattingOptions.None
        )
        {
            var bytesArray = bytes.ToArray();
            return Convert.ToBase64String(bytesArray, 0, bytesArray.Length, options);
        }
    }
}
#endif
