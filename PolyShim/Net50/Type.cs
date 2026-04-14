#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
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
