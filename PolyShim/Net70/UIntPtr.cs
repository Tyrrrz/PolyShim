#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
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
internal static class MemberPolyfills_Net70_UIntPtr
{
    extension(UIntPtr)
    {
        // https://learn.microsoft.com/dotnet/api/system.uintptr.tryparse#system-uintptr-tryparse(system-string-system-iformatprovider-system-uintptr@)
        public static bool TryParse(string s, IFormatProvider? provider, out UIntPtr result)
        {
            if (IntPtr.Size == 4)
            {
                var success = uint.TryParse(s, provider, out var intResult);
                result = new UIntPtr(intResult);
                return success;
            }
            else
            {
                var success = ulong.TryParse(s, provider, out var longResult);
                result = new UIntPtr(longResult);
                return success;
            }
        }

#if FEATURE_MEMORY
        // https://learn.microsoft.com/dotnet/api/system.uintptr.tryparse#system-uintptr-tryparse(system-readonlyspan((system-char))-system-iformatprovider-system-uintptr@)
        public static bool TryParse(
            ReadOnlySpan<char> s,
            IFormatProvider? provider,
            out UIntPtr result
        ) => UIntPtr.TryParse(s.ToString(), provider, out result);
#endif
    }
}
#endif
