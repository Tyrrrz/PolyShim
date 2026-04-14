#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Versioning;

// https://learn.microsoft.com/dotnet/api/system.runtime.versioning.unsupportedosplatformguardattribute
[AttributeUsage(
    AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property,
    AllowMultiple = true,
    Inherited = false
)]
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class UnsupportedOSPlatformGuardAttribute(string platformName)
    // OSPlatformAttribute's constructor is not accessible where that type is natively defined
#if !(NETCOREAPP && NET5_0_OR_GREATER)
    : OSPlatformAttribute(platformName);
#else
    : Attribute
{
    public string PlatformName { get; } = platformName;
}
#endif
#endif
