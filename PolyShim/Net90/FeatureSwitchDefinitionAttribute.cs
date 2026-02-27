#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS9216

// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.featureswitchdefinitionattribute
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
[AttributeUsage(AttributeTargets.Property)]
internal class FeatureSwitchDefinitionAttribute(string switchName) : Attribute
{
    public string SwitchName { get; } = switchName;
}
#endif
