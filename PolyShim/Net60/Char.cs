#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net60_Char
{
    extension(char)
    {
        // https://learn.microsoft.com/dotnet/api/system.char.isascii
        public static bool IsAscii(char c) => (uint)c <= '\x7f';
    }
}
#endif
