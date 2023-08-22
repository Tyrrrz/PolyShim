#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETSTANDARD && !NETSTANDARD2_0_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Reflection;

internal static partial class PolyfillExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.type.issubclassof
    public static bool IsSubclassOf(this Type type, Type otherType)
    {
        var currentType = type;

        if (currentType == otherType)
            return false;

        while (currentType != null)
        {
            if (currentType == otherType)
                return true;

            currentType = currentType.GetTypeInfo().BaseType;
        }

        return false;
    }
}
#endif
