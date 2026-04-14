#if (NETCOREAPP && !NET7_0_OR_GREATER) || (NETFRAMEWORK) || (NETSTANDARD)
#nullable enable
#pragma warning disable CS0436

using System.IO;
using System.Diagnostics.CodeAnalysis;

// No file I/O on .NET Standard prior to 1.3
#if !NETSTANDARD || NETSTANDARD1_3_OR_GREATER
#if !POLYSHIM_INCLUDE_COVERAGE
[ExcludeFromCodeCoverage]
#endif
internal static class MemberPolyfills_Net70_Path
{
    extension(Path)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.path.exists
        public static bool Exists(string? path) =>
            !string.IsNullOrEmpty(path) && (File.Exists(path) || Directory.Exists(path));
    }
}
#endif
#endif
