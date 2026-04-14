#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.Diagnostics.CodeAnalysis;

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
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class SkipLocalsInitAttribute : Attribute;
#endif
