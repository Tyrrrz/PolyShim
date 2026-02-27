#if (NETCOREAPP && !NET5_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net50_UIntPtr
{
    extension(UIntPtr)
    {
        // https://learn.microsoft.com/dotnet/api/system.uintptr.parse#system-uintptr-parse(system-string-system-globalization-numberstyles-system-iformatprovider)
        public static UIntPtr Parse(string s, NumberStyles style, IFormatProvider? provider) =>
            new(
                UIntPtr.Size == 4 ? uint.Parse(s, style, provider) : ulong.Parse(s, style, provider)
            );

        // https://learn.microsoft.com/dotnet/api/system.uintptr.parse#system-uintptr-parse(system-string-system-iformatprovider)
        public static UIntPtr Parse(string s, IFormatProvider? provider) =>
            new(UIntPtr.Size == 4 ? uint.Parse(s, provider) : ulong.Parse(s, provider));

        // https://learn.microsoft.com/dotnet/api/system.uintptr.parse#system-uintptr-parse(system-string-system-globalization-numberstyles)
        public static UIntPtr Parse(string s, NumberStyles style) =>
            new(UIntPtr.Size == 4 ? uint.Parse(s, style) : ulong.Parse(s, style));

        // https://learn.microsoft.com/dotnet/api/system.uintptr.parse#system-uintptr-parse(system-string)
        public static UIntPtr Parse(string s) =>
            new(UIntPtr.Size == 4 ? uint.Parse(s) : ulong.Parse(s));

        // https://learn.microsoft.com/dotnet/api/system.uintptr.tryparse#system-uintptr-tryparse(system-string-system-uintptr@)
        public static bool TryParse(string? s, out UIntPtr result)
        {
            if (UIntPtr.Size == 4)
            {
                var success = uint.TryParse(s, out var intResult);
                result = new UIntPtr(intResult);
                return success;
            }
            else
            {
                var success = ulong.TryParse(s, out var longResult);
                result = new UIntPtr(longResult);
                return success;
            }
        }
    }
}
#endif
