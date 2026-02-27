#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Runtime.InteropServices;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore30_OSPlatform
{
    extension(OSPlatform)
    {
        // https://learn.microsoft.com/dotnet/api/system.runtime.interopservices.osplatform.freebsd
        public static OSPlatform FreeBSD => OSPlatform.Create("FreeBSD");
    }
}
#endif
