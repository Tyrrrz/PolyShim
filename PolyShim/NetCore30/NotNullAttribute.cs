#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.notnullattribute
[AttributeUsage(
    AttributeTargets.Field
        | AttributeTargets.Parameter
        | AttributeTargets.Property
        | AttributeTargets.ReturnValue
)]
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class NotNullAttribute : Attribute;
#endif
