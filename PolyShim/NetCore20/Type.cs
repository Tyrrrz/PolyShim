#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore20_Type
{
    extension(Type type)
    {
        // https://learn.microsoft.com/dotnet/api/system.type.isassignablefrom
        public bool IsAssignableFrom(Type otherType) =>
            type.GetTypeInfo().IsAssignableFrom(otherType.GetTypeInfo());

        // https://learn.microsoft.com/dotnet/api/system.type.issubclassof
        public bool IsSubclassOf(Type otherType)
        {
            var currentType = type;

            if (currentType == otherType)
                return false;

            while (currentType is not null)
            {
                if (currentType == otherType)
                    return true;

                currentType = currentType.GetTypeInfo().BaseType;
            }

            return false;
        }
    }
}
#endif
