#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.skiplocalsinitattribute
[AttributeUsage(
    AttributeTargets.Module
        | AttributeTargets.Class
        | AttributeTargets.Struct
        | AttributeTargets.Interface
        | AttributeTargets.Constructor
        | AttributeTargets.Method
        | AttributeTargets.Property
        | AttributeTargets.Event,
    Inherited = false
)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class SkipLocalsInitAttribute : Attribute;
#endif
