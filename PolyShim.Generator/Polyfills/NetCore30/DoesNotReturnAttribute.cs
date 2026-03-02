#nullable enable

// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.doesnotreturnattribute
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class DoesNotReturnAttribute : Attribute;
