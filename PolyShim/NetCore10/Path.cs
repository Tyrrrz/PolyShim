#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

using System.IO;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static class MemberPolyfills_NetCore10_Path
{
    extension(Path)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.path.combine#system-io-path-combine(system-string())
        public static string Combine(params string[] paths) =>
            paths.Aggregate(string.Empty, Path.Combine);
    }
}
#endif
