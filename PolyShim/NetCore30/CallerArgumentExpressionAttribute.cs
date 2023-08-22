#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callerargumentexpressionattribute
[AttributeUsage(AttributeTargets.Parameter)]
[ExcludeFromCodeCoverage]
internal class CallerArgumentExpressionAttribute : Attribute
{
    public string ParameterName { get; }

    public CallerArgumentExpressionAttribute(string parameterName) => ParameterName = parameterName;
}
#endif
