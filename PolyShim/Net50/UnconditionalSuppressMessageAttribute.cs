#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.unconditionalsuppressmessageattribute
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class UnconditionalSuppressMessageAttribute(string category, string checkId) : Attribute
{
    public string Category { get; } = category;

    public string CheckId { get; } = checkId;

    public string? MessageId { get; init; }

    public string? Justification { get; init; }

    public string? Scope { get; init; }

    public string? Target { get; init; }
}
#endif
