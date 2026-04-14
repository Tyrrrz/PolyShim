#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.requiresdynamiccodeattribute
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method,
    Inherited = false
)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class RequiresDynamicCodeAttribute(string message) : Attribute
{
    public string Message { get; } = message;

    public string? Url { get; init; }
}
#endif
