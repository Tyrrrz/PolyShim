#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices;

// https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.requiredmemberattribute
[AttributeUsage(
    AttributeTargets.Class
        | AttributeTargets.Field
        | AttributeTargets.Property
        | AttributeTargets.Struct,
    Inherited = false
)]
#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal class RequiredMemberAttribute : Attribute;
#endif
