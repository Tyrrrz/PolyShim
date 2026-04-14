#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.Collections;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_DictionaryEntry
{
    extension(DictionaryEntry entry)
    {
        // Used to implement tuple deconstruction, but tuple support is technically not required
        // https://learn.microsoft.com/dotnet/api/system.collections.dictionaryentry.deconstruct
        public void Deconstruct(out object key, out object? value)
        {
            key = entry.Key;
            value = entry.Value;
        }
    }
}
#endif
