#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Globalization;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_IntPtr
{
    extension(IntPtr)
    {
        // https://learn.microsoft.com/dotnet/api/system.intptr.parse#system-intptr-parse(system-string-system-globalization-numberstyles-system-iformatprovider)
        public static IntPtr Parse(string s, NumberStyles style, IFormatProvider? provider) =>
            new(IntPtr.Size == 4 ? int.Parse(s, style, provider) : long.Parse(s, style, provider));

        // https://learn.microsoft.com/dotnet/api/system.intptr.parse#system-intptr-parse(system-string-system-iformatprovider)
        public static IntPtr Parse(string s, IFormatProvider? provider) =>
            new(IntPtr.Size == 4 ? int.Parse(s, provider) : long.Parse(s, provider));

        // https://learn.microsoft.com/dotnet/api/system.intptr.parse#system-intptr-parse(system-string-system-globalization-numberstyles)
        public static IntPtr Parse(string s, NumberStyles style) =>
            new(IntPtr.Size == 4 ? int.Parse(s, style) : long.Parse(s, style));

        // https://learn.microsoft.com/dotnet/api/system.intptr.parse#system-intptr-parse(system-string)
        public static IntPtr Parse(string s) =>
            new(IntPtr.Size == 4 ? int.Parse(s) : long.Parse(s));

        // https://learn.microsoft.com/dotnet/api/system.intptr.tryparse#system-intptr-tryparse(system-string-system-intptr@)
        public static bool TryParse(string? s, out IntPtr result)
        {
            if (IntPtr.Size == 4)
            {
                var success = int.TryParse(s, out var intResult);
                result = new IntPtr(intResult);
                return success;
            }
            else
            {
                var success = long.TryParse(s, out var longResult);
                result = new IntPtr(longResult);
                return success;
            }
        }
    }
}
#endif
