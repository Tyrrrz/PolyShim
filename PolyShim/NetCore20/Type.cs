#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
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
