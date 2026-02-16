#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
using System.Diagnostics.CodeAnalysis;

// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.setsrequiredmembersattribute
[AttributeUsage(AttributeTargets.Constructor)]
[ExcludeFromCodeCoverage]
internal class SetsRequiredMembersAttribute : Attribute;
#endif
