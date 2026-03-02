#nullable enable

// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/dotnet/api/system.diagnostics.codeanalysis.maybenullwhenattribute
[AttributeUsage(AttributeTargets.Parameter)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class MaybeNullWhenAttribute(bool returnValue) : Attribute
{
    public bool ReturnValue { get; } = returnValue;
}
