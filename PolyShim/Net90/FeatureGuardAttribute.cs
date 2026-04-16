#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436
#pragma warning disable CS9216

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.featureguardattribute
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
internal class FeatureGuardAttribute(Type featureType) : Attribute
{
    public Type FeatureType { get; } = featureType;
}
#endif
