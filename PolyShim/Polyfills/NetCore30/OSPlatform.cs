#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

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
