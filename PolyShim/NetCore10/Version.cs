#if (NETFRAMEWORK && !NET40_OR_GREATER)
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
internal static class MemberPolyfills_NetCore10_Version
{
    extension(Version)
    {
        // https://learn.microsoft.com/dotnet/api/system.version.parse
        public static Version Parse(string input) => new(input);

        // https://learn.microsoft.com/dotnet/api/system.version.tryparse
        public static bool TryParse(string? input, out Version? result)
        {
            result = null;
            if (input is null)
                return false;

            try
            {
                result = new Version(input);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
#endif
