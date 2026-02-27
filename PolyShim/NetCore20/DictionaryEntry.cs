#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
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
