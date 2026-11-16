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
    extension(char c)
    {
        // https://learn.microsoft.com/dotnet/api/system.char.equals
        public bool Equals(char other, StringComparison comparisonType) =>
            comparisonType switch
            {
                StringComparison.Ordinal => c == other,
                StringComparison.OrdinalIgnoreCase =>
                    c == other
                    || char.ToUpperInvariant(c) == char.ToUpperInvariant(other)
                    || char.ToLowerInvariant(c) == char.ToLowerInvariant(other),
                _ => c.ToString().Equals(other.ToString(), comparisonType),
            };
    }
}
#endif
