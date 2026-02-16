#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
using System.Diagnostics.CodeAnalysis;

// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.requiresdynamiccodeattribute
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method,
    Inherited = false
)]
[ExcludeFromCodeCoverage]
internal class RequiresDynamicCodeAttribute(string message) : Attribute
{
    public string Message { get; } = message;

    public string? Url { get; init; }
}
#endif
