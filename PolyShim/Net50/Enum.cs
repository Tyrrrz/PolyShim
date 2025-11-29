#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;

internal static partial class PolyfillExtensions
{
    extension(Enum)
    {
        // https://learn.microsoft.com/dotnet/api/system.enum.getnames#system-enum-getnames-1
        public static string[] GetNames<T>()
            where T : struct, Enum => Enum.GetNames(typeof(T));

        // https://learn.microsoft.com/dotnet/api/system.enum.getvalues#system-enum-getvalues-1
        public static T[] GetValues<T>()
            where T : struct, Enum => (T[])Enum.GetValues(typeof(T));
    }
}
#endif
