#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.dynamicallyaccessedmembersattribute
[AttributeUsage(
    AttributeTargets.Field
        | AttributeTargets.ReturnValue
        | AttributeTargets.GenericParameter
        | AttributeTargets.Parameter
        | AttributeTargets.Property
        | AttributeTargets.Method
        | AttributeTargets.Class
        | AttributeTargets.Interface
        | AttributeTargets.Struct,
    Inherited = false
)]
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class DynamicallyAccessedMembersAttribute(DynamicallyAccessedMemberTypes memberTypes)
    : Attribute
{
    public DynamicallyAccessedMemberTypes MemberTypes { get; } = memberTypes;
}
#endif
