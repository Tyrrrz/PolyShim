#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.membernotnullwhenattribute
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class MemberNotNullWhenAttribute(bool returnValue, params string[] members) : Attribute
{
    public bool ReturnValue { get; } = returnValue;

    public string[] Members { get; } = members;
}
#endif
