#if (NETCOREAPP && !NETCOREAPP2_1_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;

internal static partial class PolyfillExtensions2
{
    extension(DateTimeOffset)
    {
        // https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.unixepoch?view=net-9.0
        public static DateTimeOffset UnixEpoch => new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
    }
}
#endif
