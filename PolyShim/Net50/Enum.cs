#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_Enum
{
    extension(Enum)
    {
        // https://learn.microsoft.com/dotnet/api/system.enum.isdefined#system-enum-isdefined-1(-0)
        public static bool IsDefined<T>(T value)
            where T : struct, Enum => Enum.IsDefined(typeof(T), value);

        // https://learn.microsoft.com/dotnet/api/system.enum.getnames#system-enum-getnames-1
        public static string[] GetNames<T>()
            where T : struct, Enum => Enum.GetNames(typeof(T));

        // https://learn.microsoft.com/dotnet/api/system.enum.getvalues#system-enum-getvalues-1
        public static T[] GetValues<T>()
            where T : struct, Enum => (T[])Enum.GetValues(typeof(T));
    }
}
#endif
