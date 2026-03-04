#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
