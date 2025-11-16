#if (NETCOREAPP && !NETCOREAPP3_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.Runtime.InteropServices;

internal static partial class PolyfillExtensions
{
    extension(OSPlatform)
    {
        public static OSPlatform FreeBSD => OSPlatform.Create("FreeBSD");
    }
}
#endif
