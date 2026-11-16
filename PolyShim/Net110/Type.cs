#if (NETCOREAPP && !NET11_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net110_Type
{
    extension(Type type)
    {
        // https://learn.microsoft.com/dotnet/api/system.type.getnullableunderlyingtype
        public Type? GetNullableUnderlyingType()
        {
            ArgumentNullException.ThrowIfNull(type);
            return Nullable.GetUnderlyingType(type);
        }
    }
}
#endif
