﻿#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Versioning;

// https://learn.microsoft.com/en-us/dotnet/api/system.runtime.versioning.obsoletedosplatformattribute
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
[ExcludeFromCodeCoverage]
internal class ObsoletedOSPlatformAttribute(string platformName, string? message = null)
    // OSPlatformAttribute's constructor is not accessible where that type is natively defined
#if !(NETCOREAPP && NET5_0_OR_GREATER)
    : OSPlatformAttribute(platformName)
#else
    : Attribute
#endif
{
#if (NETCOREAPP && NET5_0_OR_GREATER)
    public string PlatformName { get; } = platformName;
#endif

    public string? Message { get; } = message;

    public string? Url { get; init; }
}
#endif
