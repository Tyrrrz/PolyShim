#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.membernotnullattribute
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class MemberNotNullAttribute(params string[] members) : Attribute
{
    public string[] Members { get; } = members;
}
#endif
