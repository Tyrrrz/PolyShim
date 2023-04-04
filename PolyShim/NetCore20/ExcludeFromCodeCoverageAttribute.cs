#if (NETCOREAPP1_0_OR_GREATER && !NETCOREAPP2_0_OR_GREATER) || (NET20_OR_GREATER && !NET40_OR_GREATER) || (NETSTANDARD1_0_OR_GREATER && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis.excludefromcodecoverageattribute
[AttributeUsage(
    AttributeTargets.Assembly |
    AttributeTargets.Class |
    AttributeTargets.Constructor |
    AttributeTargets.Event |
    AttributeTargets.Method |
    AttributeTargets.Property |
    AttributeTargets.Struct,
    Inherited = false)]
[ExcludeFromCodeCoverage]
internal class ExcludeFromCodeCoverageAttribute : Attribute
{
}
#endif