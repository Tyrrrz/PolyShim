#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Reflection;

internal static partial class PolyfillExtensions
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.type.isassignableto
    public static bool IsAssignableTo(this Type type, Type? otherType) =>
#if NETSTANDARD && !NETSTANDARD2_0_OR_GREATER
        otherType?.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()) == true;
#else
        otherType?.IsAssignableFrom(type) == true;
#endif
}
#endif
