#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_Type
{
    extension(Type type)
    {
        // https://learn.microsoft.com/dotnet/api/system.type.isassignableto
        public bool IsAssignableTo(Type? otherType) => otherType?.IsAssignableFrom(type) == true;
    }
}
#endif
