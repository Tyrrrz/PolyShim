#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.notnullifnotnullattribute
[AttributeUsage(
    AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue
)]
[ExcludeFromCodeCoverage]
internal class NotNullIfNotNullAttribute(string parameterName) : Attribute
{
    public string ParameterName { get; } = parameterName;
}
#endif
