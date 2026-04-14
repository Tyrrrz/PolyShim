#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Versioning;

// https://learn.microsoft.com/dotnet/api/system.runtime.versioning.supportedosplatformattribute
[AttributeUsage(
    AttributeTargets.Assembly
        | AttributeTargets.Class
        | AttributeTargets.Constructor
        | AttributeTargets.Enum
        | AttributeTargets.Event
        | AttributeTargets.Field
        | AttributeTargets.Interface
        | AttributeTargets.Method
        | AttributeTargets.Module
        | AttributeTargets.Property
        | AttributeTargets.Struct,
    AllowMultiple = true,
    Inherited = false
)]
#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class SupportedOSPlatformAttribute(string platformName)
    : OSPlatformAttribute(platformName);
#endif
