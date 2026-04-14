#if (NETFRAMEWORK && !NET40_OR_GREATER)
#nullable enable
#pragma warning disable CS0436

using System.IO;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
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
