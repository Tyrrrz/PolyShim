﻿#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(
    AttributeTargets.Constructor
        | AttributeTargets.Event
        | AttributeTargets.Method
        | AttributeTargets.Property,
    Inherited = false
)]
[ExcludeFromCodeCoverage]
internal class RequiresAssemblyFilesAttribute(string? message) : Attribute
{
    public RequiresAssemblyFilesAttribute()
        : this(null) { }

    public string? Message { get; } = message;

    public string? Url { get; init; }
}
#endif
