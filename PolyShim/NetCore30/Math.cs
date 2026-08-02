#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore30_Math
{
    extension(Math)
    {
        // https://learn.microsoft.com/dotnet/api/system.math.log2
        public static double Log2(double x) => Math.Log(x, 2);
    }
}
#endif
