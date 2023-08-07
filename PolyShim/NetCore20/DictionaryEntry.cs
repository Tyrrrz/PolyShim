#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Collections;

internal static partial class PolyfillExtensions
{
    // Used to implement tuple deconstruction, but tuple support is technically not required
    // https://learn.microsoft.com/en-us/dotnet/api/system.collections.dictionaryentry.deconstruct
    public static void Deconstruct(this DictionaryEntry entry, out object key, out object? value)
    {
        key = entry.Key;
        value = entry.Value;
    }
}
#endif