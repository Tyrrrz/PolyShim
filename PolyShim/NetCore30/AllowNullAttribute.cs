#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable

// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.allownullattribute
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class AllowNullAttribute : Attribute;
#endif
