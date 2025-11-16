#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.membernotnullwhenattribute
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
[ExcludeFromCodeCoverage]
internal class MemberNotNullWhenAttribute(bool returnValue, params string[] members) : Attribute
{
    public bool ReturnValue { get; } = returnValue;

    public string[] Members { get; } = members;
}
#endif
