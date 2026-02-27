#if !FEATURE_VALUETUPLE
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

namespace System.Runtime.CompilerServices;

#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.tupleelementnamesattribute
[AttributeUsage(
    AttributeTargets.Class
        | AttributeTargets.Event
        | AttributeTargets.Field
        | AttributeTargets.Parameter
        | AttributeTargets.Property
        | AttributeTargets.ReturnValue
        | AttributeTargets.Struct
)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class TupleElementNamesAttribute(string[] transformNames) : Attribute
{
    public string[] TransformNames { get; } = transformNames;
}
#endif
