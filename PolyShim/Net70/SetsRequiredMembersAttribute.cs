#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis.setsrequiredmembersattribute
[AttributeUsage(AttributeTargets.Constructor)]
[ExcludeFromCodeCoverage]
internal class SetsRequiredMembersAttribute : Attribute;
#endif
