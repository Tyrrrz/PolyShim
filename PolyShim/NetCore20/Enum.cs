#if (NETCOREAPP && !NETCOREAPP2_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
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
internal static class MemberPolyfills_NetCore20_Enum
{
    extension(Enum)
    {
        // https://learn.microsoft.com/dotnet/api/system.enum.parse#system-enum-parse-1(system-string-system-boolean)
        public static T Parse<T>(string value, bool ignoreCase)
            where T : struct, Enum => (T)Enum.Parse(typeof(T), value, ignoreCase);

        // https://learn.microsoft.com/dotnet/api/system.enum.parse#system-enum-parse-1(system-string)
        public static T Parse<T>(string value)
            where T : struct, Enum => (T)Enum.Parse(typeof(T), value);
    }
}
#endif
