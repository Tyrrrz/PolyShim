#if (NETCOREAPP1_0_OR_GREATER && !NET7_0_OR_GREATER) || (NET20_OR_GREATER) || (NETSTANDARD1_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis.setsrequiredmembersattribute
[AttributeUsage(AttributeTargets.Constructor)]
[ExcludeFromCodeCoverage]
internal class SetsRequiredMembersAttribute : Attribute
{
}
#endif