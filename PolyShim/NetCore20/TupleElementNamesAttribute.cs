#if !FEATURE_VALUETUPLE
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Runtime.CompilerServices;

using System.Diagnostics.CodeAnalysis;

// https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.tupleelementnamesattribute
[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Event |
    AttributeTargets.Field |
    AttributeTargets.Parameter |
    AttributeTargets.Property |
    AttributeTargets.ReturnValue |
    AttributeTargets.Struct
)]
[ExcludeFromCodeCoverage]
internal class TupleElementNamesAttribute : Attribute
{
    public string[] TransformNames { get; }

    public TupleElementNamesAttribute(string[] transformNames) =>
        TransformNames = transformNames;
}
#endif