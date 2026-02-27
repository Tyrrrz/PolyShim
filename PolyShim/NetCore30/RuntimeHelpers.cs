#if !FEATURE_INDEXRANGE
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

namespace System.Runtime.CompilerServices;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.runtimehelpers
internal static class RuntimeHelpers
{
    // Enables range-based slice on arrays
    // https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.runtimehelpers.getsubarray
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
