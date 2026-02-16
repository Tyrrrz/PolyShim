#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
using System.Diagnostics.CodeAnalysis;

// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.doesnotreturnifattribute
[AttributeUsage(AttributeTargets.Parameter)]
[ExcludeFromCodeCoverage]
internal class DoesNotReturnIfAttribute(bool parameterValue) : Attribute
{
    public bool ParameterValue { get; } = parameterValue;
}
#endif
