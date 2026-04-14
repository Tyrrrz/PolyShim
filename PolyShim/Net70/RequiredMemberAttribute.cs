#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.requiredmemberattribute
[AttributeUsage(
    AttributeTargets.Class
        | AttributeTargets.Field
        | AttributeTargets.Property
        | AttributeTargets.Struct,
    Inherited = false
)]
#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class RequiredMemberAttribute : Attribute;
#endif
