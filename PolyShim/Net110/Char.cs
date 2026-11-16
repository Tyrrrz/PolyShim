#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net110_Char
{
    extension(char ch)
    {
        // https://learn.microsoft.com/dotnet/api/system.char.equals?system-char-equals(system-char-system-stringcomparison)
        public bool Equals(char other, StringComparison comparisonType) =>
            string.Equals($"{ch}", $"{other}", comparisonType);
    }
}
#endif
