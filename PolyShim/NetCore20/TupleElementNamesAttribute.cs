#if !FEATURE_VALUETUPLE
#nullable enable
#pragma warning disable CS0436

namespace System.Runtime.CompilerServices;

using System.Diagnostics.CodeAnalysis;

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
