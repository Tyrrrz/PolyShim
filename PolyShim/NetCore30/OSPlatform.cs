#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
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
