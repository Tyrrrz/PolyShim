#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

namespace System.Runtime.Versioning;

// https://learn.microsoft.com/dotnet/api/system.runtime.versioning.unsupportedosplatformguardattribute
[AttributeUsage(
    AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property,
    AllowMultiple = true,
    Inherited = false
)]
#if !POLYFILL_COVERAGE
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
