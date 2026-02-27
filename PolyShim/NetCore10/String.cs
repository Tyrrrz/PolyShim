#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Linq;
#if !POLYFILL_COVERAGE
using System.Diagnostics.CodeAnalysis;
#endif

#if !POLYFILL_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_NetCore10_String
{
    extension(string)
    {
        // https://learn.microsoft.com/dotnet/api/system.string.isnullorwhitespace
        public static bool IsNullOrWhiteSpace(string? value) =>
            value is null || value.All(char.IsWhiteSpace);
    }
}
#endif
