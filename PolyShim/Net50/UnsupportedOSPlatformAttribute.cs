#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Versioning;

// https://learn.microsoft.com/dotnet/api/system.runtime.versioning.unsupportedosplatformattribute
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
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class UnsupportedOSPlatformAttribute(string platformName, string? message = null)
    : OSPlatformAttribute(platformName)
{
    public string? Message { get; } = message;
}
#endif
