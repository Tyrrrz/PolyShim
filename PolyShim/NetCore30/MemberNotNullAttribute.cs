#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis.membernotnullattribute
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
[ExcludeFromCodeCoverage]
internal class MemberNotNullAttribute : Attribute
{
    public string[] Members { get; }

    public MemberNotNullAttribute(string member) => Members = new[] { member };

    public MemberNotNullAttribute(params string[] members) => Members = members;
}
#endif
