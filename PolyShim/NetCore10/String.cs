#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
#pragma warning disable CS0436
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

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

        // https://learn.microsoft.com/dotnet/api/system.string.join#system-string-join(system-string-system-object())
        public static string Join(string? separator, params object?[] values) =>
            string.Join(separator, values.Select(v => v?.ToString()).ToArray());

        // https://learn.microsoft.com/dotnet/api/system.string.join#system-string-join-1(system-string-system-collections-generic-ienumerable((-0)))
        public static string Join<T>(string? separator, IEnumerable<T> values) =>
            string.Join(separator, values.Select(v => ((object?)v)?.ToString()).ToArray());
    }
}
#endif
