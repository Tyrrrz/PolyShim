#if (NETCOREAPP && !NET9_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS9216
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.overloadresolutionpriorityattribute
[ExcludeFromCodeCoverage]
[AttributeUsage(
    AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property,
    Inherited = false
)]
internal class OverloadResolutionPriorityAttribute(int priority) : Attribute
{
    public int Priority { get; } = priority;
}
#endif
