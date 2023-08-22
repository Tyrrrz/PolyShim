#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.compilerfeaturerequiredattribute
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
[ExcludeFromCodeCoverage]
internal partial class CompilerFeatureRequiredAttribute : Attribute
{
    public string FeatureName { get; }

    public bool IsOptional { get; init; }

    public CompilerFeatureRequiredAttribute(string featureName) => FeatureName = featureName;
}

internal partial class CompilerFeatureRequiredAttribute
{
    public const string RefStructs = nameof(RefStructs);

    public const string RequiredMembers = nameof(RequiredMembers);
}
#endif
