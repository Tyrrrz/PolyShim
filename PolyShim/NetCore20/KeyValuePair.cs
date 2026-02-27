#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_KeyValuePair
{
    extension<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
    {
        // Used to implement tuple deconstruction, but tuple support is technically not required
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.keyvaluepair-2.deconstruct
        public void Deconstruct(out TKey key, out TValue value)
        {
            key = pair.Key;
            value = pair.Value;
        }
    }
}
#endif
