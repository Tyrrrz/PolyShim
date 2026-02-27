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
internal static class MemberPolyfills_Net70_IntPtr
{
    extension(IntPtr)
    {
        // https://learn.microsoft.com/dotnet/api/system.intptr.tryparse#system-intptr-tryparse(system-string-system-iformatprovider-system-intptr@)
        public static bool TryParse(string s, IFormatProvider? provider, out IntPtr result)
        {
            if (IntPtr.Size == 4)
            {
                var success = int.TryParse(s, provider, out var intResult);
                result = new IntPtr(intResult);
                return success;
            }
            else
            {
                var success = long.TryParse(s, provider, out var longResult);
                result = new IntPtr(longResult);
                return success;
            }
        }
    }
}
#endif
