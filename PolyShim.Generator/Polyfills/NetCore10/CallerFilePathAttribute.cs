#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.callerfilepathattribute
[AttributeUsage(AttributeTargets.Parameter)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class CallerFilePathAttribute : Attribute;
