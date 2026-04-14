#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.asyncmethodbuilderattribute
[AttributeUsage(
    AttributeTargets.Class
        | AttributeTargets.Struct
        | AttributeTargets.Interface
        | AttributeTargets.Delegate
        | AttributeTargets.Enum,
    Inherited = false,
    AllowMultiple = false
)]
#if !POLYSHIM_EXCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class AsyncMethodBuilderAttribute(Type builderType) : Attribute
{
    public Type BuilderType { get; } = builderType;
}
#endif
