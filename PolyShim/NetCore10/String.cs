#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Linq;

internal static partial class PolyfillExtensions
{
    extension(string)
    {
        // https://learn.microsoft.com/dotnet/api/system.string.isnullorwhitespace
        public static bool IsNullOrWhiteSpace(string? value) =>
            value is null || value.All(char.IsWhiteSpace);
    }
}
#endif
