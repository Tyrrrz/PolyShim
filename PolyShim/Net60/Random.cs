#if (NETCOREAPP && !NET6_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System;

internal static partial class PolyfillExtensions
{
    [ThreadStatic]
    private static Random? _threadRandom;

    extension(Random)
    {
        // https://learn.microsoft.com/dotnet/api/system.random.shared
        public static Random Shared => _threadRandom ??= new Random();
    }
}
#endif
