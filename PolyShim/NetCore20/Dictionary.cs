#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore20_Dictionary
{
    extension<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        // https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2.tryadd
        public bool TryAdd(TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                return false;

            dictionary.Add(key, value);
            return true;
        }
    }
}
#endif
