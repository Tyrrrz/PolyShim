#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System;
using System.Diagnostics.CodeAnalysis;

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_Guid
{
    extension(Guid)
    {
        // https://learn.microsoft.com/dotnet/api/system.guid.parse
        public static Guid Parse(string input) => new(input);

        // https://learn.microsoft.com/dotnet/api/system.guid.tryparse
        public static bool TryParse(string? input, out Guid result)
        {
            if (input is null)
            {
                result = default;
                return false;
            }

            try
            {
                result = Guid.Parse(input);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}
#endif
