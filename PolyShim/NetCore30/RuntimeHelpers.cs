#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

[ExcludeFromCodeCoverage]
internal static class RuntimeHelpers
{
    // Enables range-based slice on arrays
    // https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.runtimehelpers.getsubarray
    public static T[] GetSubArray<T>(T[] array, Range range)
    {
        var start = range.Start.GetOffset(array.Length);
        var end = range.End.GetOffset(array.Length);
        var length = end - start;

        var result = new T[length];
        Array.Copy(array, start, result, 0, length);

        return result;
    }
}
#endif
