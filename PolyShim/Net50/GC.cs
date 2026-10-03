#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_GC
{
    extension(GC)
    {
        // https://learn.microsoft.com/dotnet/api/system.gc.allocateuninitializedarray#system-gc-allocateuninitializedarray-1(system-int32-system-boolean)
        // Older runtimes don't support uninitialized allocations or pinned object heap, so this
        // always falls back to a regular zero-initialized array, ignoring the `pinned` parameter.
        public static T[] AllocateUninitializedArray<T>(int length, bool pinned = false) =>
            new T[length];
    }
}
#endif
