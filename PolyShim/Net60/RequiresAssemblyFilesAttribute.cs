#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.requiresassemblyfilesattribute
[AttributeUsage(
    AttributeTargets.Constructor
        | AttributeTargets.Event
        | AttributeTargets.Method
        | AttributeTargets.Property,
    Inherited = false
)]
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class RequiresAssemblyFilesAttribute(string? message) : Attribute
{
    public RequiresAssemblyFilesAttribute()
        : this(null) { }

    public string? Message { get; } = message;

    public string? Url { get; init; }
}
#endif
