#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callerargumentexpressionattribute
[AttributeUsage(AttributeTargets.Parameter)]
[ExcludeFromCodeCoverage]
internal class CallerArgumentExpressionAttribute : Attribute
{
    public string ParameterName { get; }

    public CallerArgumentExpressionAttribute(string parameterName) =>
        ParameterName = parameterName;
}
#endif