#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.requiresunreferencedcodeattribute
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method,
    Inherited = false
)]
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class RequiresUnreferencedCodeAttribute(string message) : Attribute
{
    public string Message { get; } = message;

    public string? Url { get; init; }
}
#endif
